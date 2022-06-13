using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using NetLimiter.Service;

public static class Program {
    #region Fields
    // NetLimiter service:
    private static NLClient _client;
    private static NodeLoader _nodeLoader;

    // Args:
    private static int _pid;
    private static string _process;
    private static List<(int Start, int End)> _portInclusionRanges = new();
    private static List<(int Start, int End)> _portExclusionRanges = new();
    private static bool _listen;
    #endregion

    public static void Main(string[] args) {
        // Ex: KillConnection.exe -process chrome.exe -port 80 -port 443
        // Ex: KillConnection.exe -listen

        #if DEBUG
        args = new[] { "-listen" };
        #endif

        for (var i = 0; i < args.Length; i++) {
            if (args.GetArgument(i, "-pid"))
                args.GetArgument(++i, out _pid);
            else if (args.GetArgument(i, "-port") && args.GetArgument(++i, out int port))
                _portInclusionRanges.Add((port, port));
            else if (args.GetArgument(i, "-process"))
                _process = args[++i];
            else if (args.GetArgument(i, "-listen"))
                _listen = true;
        }

        if (_pid == default && _portInclusionRanges.Count == 0 && _process == default && _listen == default) {
            Console.WriteLine("[!] Invalid KillConnection.exe usage!");
            Console.WriteLine();
            Console.WriteLine("  Expected arguments (commandline mode):");
            Console.WriteLine("    -pid 1234           | Kills connections owned by the specified process (via PID.)");
            Console.WriteLine("    -process abc.exe    | Kills connections owned by the specified process (via name.)");
            Console.WriteLine("    -port 443           | Kills connections established on the specified remote port.");
            Console.WriteLine();
            Console.WriteLine("  Expected arguments (service mode):");
            Console.WriteLine("    -listen             | Listens to kill commands issued on the Windows message queue.");
            return;
        }

        _client = new NLClient();
        _client.Connect();
        
        Console.WriteLine("[+] Connected to NetLimiter service.");

        _nodeLoader = _client.CreateNodeLoader();
        _nodeLoader.Connections.Nodes.Created += OnNodeCreated;
        _nodeLoader.SelectAllNodes();

        if (!_listen) {
            _nodeLoader.Load();
            Console.WriteLine("Done!");
            Console.WriteLine();
            return;
        }

        using var window = new Form();
        MessageHandler.Listen(window.Handle);
        MessageHandler.Bind(Assembly.GetExecutingAssembly());
        
        Console.WriteLine("[+] Listening for window messages.");

        Application.Run();
    }

    [MessageHandler(0x00000000)]
    private static void OnKillConnections(WindowMessageEventArgs args) {
        using var memory = new MemoryStream(args.Data);
        using var stream = new BinaryReader(memory);
        
        _pid = stream.ReadInt32();
        _process = null;
        _portInclusionRanges.Clear();
        _portExclusionRanges.Clear();
        
        // Included individual ports.
        for (var i = stream.ReadInt32(); i > 0; i--) {
            var port = stream.ReadUInt16();
            _portInclusionRanges.Add((port, port));
        }

        // Included port ranges.
        for (var i = stream.ReadInt32(); i > 0; i--)
            _portInclusionRanges.Add((stream.ReadUInt16(), stream.ReadUInt16()));
        
        // Excluded individual ports.
        for (var i = stream.ReadInt32(); i > 0; i--) {
            var port = stream.ReadUInt16();
            _portExclusionRanges.Add((port, port));
        }

        // Excluded port ranges.
        for (var i = stream.ReadInt32(); i > 0; i--)
            _portExclusionRanges.Add((stream.ReadUInt16(), stream.ReadUInt16()));

        // Kill connections.
        Console.WriteLine();
        Console.WriteLine("[+] Received kill command.");
        _nodeLoader.Clear();
        _nodeLoader.Load();
    }

    private static void OnNodeCreated(object sender, ConnectionNodeEventArgs e) {
        var connection = e.Node;
        if (connection.ConnectionType != ConnectionType.Connect)
            return;

        if (connection.RemoteAddress.IsLoopback)
            return;

        var instance = connection.Parent;
        if (_pid != default && instance.ProcessId != _pid)
            return;
        
        if (_process != default && !string.Equals(Path.GetFileName(instance.Parent.FilePath), Path.GetFileName(_process), StringComparison.OrdinalIgnoreCase))
            return;

        var port = connection.RemotePort;
        if (_portExclusionRanges.Count > 0 && _portExclusionRanges.Any(portRange => port >= portRange.Start && port <= portRange.End))
            return;

        if (_portInclusionRanges.Count > 0 && !_portInclusionRanges.Any(portRange => port >= portRange.Start && port <= portRange.End))
            return;

        _client.KillCnns(e.Node.Id);
        Console.WriteLine($"Killed {connection.RemoteAddress}:{connection.RemotePort}");
    }
}
