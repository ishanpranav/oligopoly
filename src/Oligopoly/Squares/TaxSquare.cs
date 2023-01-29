using System;
using System.IO;
using Oligopoly.Writers;

namespace Oligopoly.Squares;

public class TaxSquare : Square
{
    public TaxSquare(string name, int cost)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (cost <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cost));
        }

        Name = name;
        Cost = cost;
    }

    /// <inheritdoc/>
    public override string Name { get; }

    /// <inheritdoc/>
    public override SquareType Type
    {
        get
        {
            return SquareType.Tax;
        }
    }

    public int Cost { get; }

    /// <inheritdoc/>
    public override void Write(Writer writer)
    {
        base.Write(writer);

        writer.Write(Name);
        writer.Write(Cost);
    }

    internal static new TaxSquare Read(BinaryReader reader)
    {
        return new TaxSquare(reader.ReadString(), reader.ReadInt32());
    }
}
