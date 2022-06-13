using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

using static User32;

public class MessageHandler {
    #region Constants
    public const int Magic = 0x52544641; // 'AFTR'
    #endregion

    #region Fields
    private int _disposeCount;

    private static readonly ConcurrentDictionary<int, WindowMessageHandler> _messageHandlers = new();
    private static readonly Dictionary<Assembly, List<MessageHandler>> _assemblyHandlers = new();
    private static IntPtr _originalWindowProc;
    private static WindowProc _customWindowProc;
    #endregion

    #region Properties
    public int Message { get; }
    public WindowMessageHandler Handler { get; }
    #endregion

    protected MessageHandler(int message, WindowMessageHandler handler) {
        Message = message;
        Handler = handler;
    }

    ~MessageHandler() {
        Dispose(false);
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    private void Dispose(bool disposing) {
        if (Interlocked.Exchange(ref _disposeCount, 1) == 0)
            OnDispose(disposing);
    }

    protected virtual void OnDispose(bool disposing) {
        if (_messageHandlers.TryGetValue(Message, out var handlers))
            _messageHandlers.TryUpdate(Message, handlers - Handler, handlers);
    }

    public static void Listen(IntPtr windowHandle) {
        _customWindowProc = HandleCustomMessage;
        _originalWindowProc = SetWindowLongPtr(windowHandle, -4, Marshal.GetFunctionPointerForDelegate(_customWindowProc));
    }

    public static MessageHandler Create(int message, WindowMessageHandler handler) {
        _messageHandlers.AddOrUpdate(message, handler, (m, h) => h + handler);
        return new MessageHandler(message, handler);
    }
    
    internal static int HandleCustomMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam) {
        if (Msg != 0x004A /* WM_COPYDATA */ || wParam.ToInt64() != Magic)
            goto Default;

        // Read the copied data.
        var data = Marshal.PtrToStructure<CopyDataStruct>(lParam);

        // Attempt to get handlers for the message.
        _messageHandlers.TryGetValue(data.Type.ToInt32(), out var handlers);

        if (handlers == null)
            goto Default;
        
        var buffer = new byte[data.Length];
        Marshal.Copy(data.Buffer, buffer, 0, data.Length);

        // Dispatch the window message.
        var args = new WindowMessageEventArgs(buffer);
        handlers(args);
        return 0;

        Default:
        return CallWindowProc(_originalWindowProc, hWnd, Msg, wParam, lParam);
    }
    
    internal static void Bind(Assembly assembly) {
        const BindingFlags methodFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        var messageHandlers = new List<MessageHandler>();

        foreach (var type in assembly.GetTypes())
        foreach (var method in type.GetMethods(methodFlags)) {
            var attribute = method.GetCustomAttribute<MessageHandlerAttribute>();
            if (attribute == null)
                continue;

            var handler = (WindowMessageHandler) method.CreateDelegate(typeof(WindowMessageHandler));

            foreach (var key in attribute.Messages)
                messageHandlers.Add(Create(key, handler));
        }

        _assemblyHandlers[assembly] = messageHandlers;
    }

    internal static void Unbind(Assembly assembly) {
        if (!_assemblyHandlers.TryGetValue(assembly, out var hotkeys))
            return;

        _assemblyHandlers.Remove(assembly);

        foreach (var hotkey in hotkeys)
            hotkey.Dispose();
    }
}
