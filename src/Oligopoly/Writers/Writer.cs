using System;
using System.Collections.Generic;
using Oligopoly.Squares;

namespace Oligopoly.Writers;

public abstract class Writer
{
    public const byte FormatByte = 228;
    public const byte VersionByte = 46;

    public abstract void Write(int value);
    public abstract void Write(SquareType value);
    public abstract void Write(string value);

    public virtual void Write(IWritable value)
    {
        ArgumentNullException.ThrowIfNull(value);

        value.Write(this);
    }

    public virtual void Write(IReadOnlyCollection<IWritable> value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Write(value.Count);

        foreach (IWritable item in value)
        {
            item.Write(this);
        }
    }

    public abstract void WriteVersion();
}
