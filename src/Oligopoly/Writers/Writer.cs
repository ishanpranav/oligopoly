using System;
using System.Collections.Generic;

namespace Oligopoly.Writers;

public abstract class Writer
{
    public const byte FormatByte = 46;
    public const byte VersionByte = 228;

    public abstract void Write(int value);

    public virtual void Write(IWritable value)
    {
        ArgumentNullException.ThrowIfNull(value);

        value.Write(this);
    }

    public virtual void Write<TWritable>(IReadOnlyCollection<TWritable> value) where TWritable : IWritable
    {
        ArgumentNullException.ThrowIfNull(value);

        Write(value.Count);

        foreach (TWritable item in value)
        {
            item.Write(this);
        }
    }

    public abstract void WriteVersion();
}
