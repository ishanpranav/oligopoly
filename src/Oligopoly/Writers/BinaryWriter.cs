using System;
using System.IO;
using Oligopoly.Squares;

namespace Oligopoly.Writers;

public class BinaryWriter : Writer, IDisposable
{
    private readonly System.IO.BinaryWriter _writer;

    private bool _disposed;

    public BinaryWriter(Stream output)
    {
        _writer = new System.IO.BinaryWriter(output);
    }

    /// <inheritdoc/>
    public override void Write(int value)
    {
        _writer.Write(value);
    }

    /// <inheritdoc/>
    public override void Write(SquareType value)
    {
        _writer.Write((byte)value);
    }

    /// <inheritdoc/>
    public override void Write(string value)
    {
        _writer.Write(value);
    }

    /// <inheritdoc/>
    public override void WriteVersion()
    {
        _writer.Write(FormatByte);
        _writer.Write(VersionByte);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _writer.Dispose();
            }

            _disposed = true;
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
