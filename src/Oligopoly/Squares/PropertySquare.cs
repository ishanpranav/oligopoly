using System;
using System.Collections.Generic;
using Oligopoly.Writers;

namespace Oligopoly.Squares;

public abstract class PropertySquare : Square
{
    protected PropertySquare(string name, int cost, IReadOnlyList<int> rents)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (cost <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cost));
        }

        Name = name;
        Cost = cost;
        Rents = rents;
    }

    /// <inheritdoc/>
    public override string Name { get; }

    /// <inheritdoc/>
    public override SquareType Type
    {
        get
        {
            return SquareType.Street;
        }
    }

    public Group? Group { get; set; }
    public int Cost { get; }
    public IReadOnlyList<int> Rents { get; }

    /// <inheritdoc/>
    public override void Write(Writer writer)
    {
        base.Write(writer);

        writer.Write(Name);
    }
}
