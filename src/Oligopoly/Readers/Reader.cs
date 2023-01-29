using System;

namespace Oligopoly.Readers;

public abstract class Reader
{
    public abstract int ReadInt32();

    public T[] ReadArray<T>(Func<T> parser)
    {
        ArgumentNullException.ThrowIfNull(parser);

        int length = ReadInt32();
        T[] results = new T[length];

        for (int i = 0; i < length; i++)
        {
            results[i] = parser();
        }

        return results;
    }

    public Board ReadBoard()
    {
        return Board.Deserialize(this);
    }

    internal Square ReadSquare()
    {
        throw new NotImplementedException();
    }

    internal Group ReadGroup()
    {
        throw new NotImplementedException();
    }
}
