using System;

namespace Oligopoly;

public class LogEventArgs : EventArgs
{
    public string Message { get; }

    public LogEventArgs(string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        Message = message;
    }
}
