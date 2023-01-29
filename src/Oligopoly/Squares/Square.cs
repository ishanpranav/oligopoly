using System;
using System.IO;
using Oligopoly.Writers;

namespace Oligopoly.Squares;

public abstract class Square : IWritable
{
    protected Square() { }

    public abstract SquareType Type { get; }

    /// <inheritdoc/>
    public virtual void Write(Writer writer)
    {
        writer.Write(Type);
    }

    internal static Square Read(BinaryReader reader)
    {
        switch ((SquareType)reader.ReadByte())
        {
            case SquareType.Start:
                return StartSquare.Read(reader);

            case SquareType.Street:
                return StreetSquare.Read(reader);

            default:
                throw new FormatException();
        }
    }
}
