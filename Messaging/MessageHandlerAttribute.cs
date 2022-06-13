using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class MessageHandlerAttribute : Attribute {
    public int[] Messages { get; }

    public MessageHandlerAttribute(params int[] messages) {
        Messages = messages;
    }
}
