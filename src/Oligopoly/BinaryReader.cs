using System;
using System.IO;
using Oligopoly.Squares;

namespace Oligopoly;

internal sealed class BinaryReader : IDisposable
{
    private readonly System.IO.BinaryReader _reader;

    private bool _disposed;

    public BinaryReader(Stream input)
    {
        _reader = new System.IO.BinaryReader(input);
    }

    private T[] ReadArray<T>(Func<T> parser)
    {
        int length = _reader.ReadInt32();
        T[] results = new T[length];

        for (int i = 0; i < length; i++)
        {
            results[i] = parser();
        }

        return results;
    }

    public Board ReadBoard()
    {
        return new Board(ReadArray(ReadSquare), ReadArray(ReadGroup));
    }

    private Square ReadSquare()
    {
        switch (_reader.ReadByte())
        {
            default:
                return new StartSquare();
        }
    }

    private Group ReadGroup()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (!_disposed)
        {
            _reader.Dispose();

            _disposed = true;
        }

        GC.SuppressFinalize(this);
    }
}
