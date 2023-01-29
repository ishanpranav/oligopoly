using System;
using System.IO;

namespace Oligopoly.Readers;

public class BinaryReader : Reader, IDisposable
{
    private readonly System.IO.BinaryReader _reader;

    private bool _disposed;

    public BinaryReader(Stream input)
    {
        _reader = new System.IO.BinaryReader(input);
    }

    public override int ReadInt32()
    {
        return _reader.ReadInt32();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _reader.Dispose();
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
