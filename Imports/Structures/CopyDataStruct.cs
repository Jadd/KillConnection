using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct CopyDataStruct {
    public IntPtr Type;
    public int Length;
    public IntPtr Buffer;
}