using System;
using System.Collections.Generic;
using Oligopoly.Writers;

namespace Oligopoly.Squares;

public class StreetSquare : Square
{
    public StreetSquare()
    {
        Name = string.Empty;
        Rents = new int[1];
    }

    public StreetSquare(string name, int cost, IReadOnlyList<int> rents)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(rents);

        if (cost <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cost));
        }

        if (rents.Count < 1)
        {
            throw new ArgumentException(string.Empty, nameof(rents));
        }

        foreach (int rent in rents)
        {
            if (rent <= 0)
            {
                throw new ArgumentException(string.Empty, nameof(rents));
            }
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
        writer.Write(Cost);
        writer.Write(Rents.Count);

        foreach (int rent in Rents)
        {
            writer.Write(rent);
        }
    }
}
