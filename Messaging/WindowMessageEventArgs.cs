using System;

public class WindowMessageEventArgs {
    public byte[] Data { get; }

    public WindowMessageEventArgs(byte[] data) {
        Data = data;
    }
}
