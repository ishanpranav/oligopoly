using System;
using System.Collections.Generic;
using Oligopoly.Writers;

namespace Oligopoly.Squares;

public class StreetSquare : PropertySquare
{
    public StreetSquare(string name, IReadOnlyList<int> rents, Group group, int cost) : base(name, rents, group)
    {
        if (cost <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cost));
        }

        Cost = cost;
    }

    /// <inheritdoc/>
    public override SquareType Type
    {
        get
        {
            return SquareType.Street;
        }
    }

    /// <inheritdoc/>
    public override int Cost { get; }

    /// <inheritdoc/>
    public override void Write(Writer writer)
    {
        base.Write(writer);

        foreach (int rent in Rents)
        {
            writer.Write(rent);
        }

        writer.Write(Group.Id);
        writer.Write(Cost);
    }
}
