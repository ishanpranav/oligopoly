using System;
using System.Collections.Generic;
using System.IO;
using Oligopoly.Writers;

namespace Oligopoly.Squares;

public class StreetSquare : Square
{
    public StreetSquare()
    {
        Name = string.Empty;
        Rents = new int[1];
    }

    public StreetSquare(string name, int cost, int improvementCost, IReadOnlyList<int> rents)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(rents);

        if (rents.Count < 1)
        {
            throw new ArgumentException(string.Empty, nameof(rents));
        }

        Name = name;
        Cost = cost;
        ImprovementCost = improvementCost;
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
    public int ImprovementCost { get; }
    public IReadOnlyList<int> Rents { get; }

    /// <inheritdoc/>
    public override void Write(Writer writer)
    {
        base.Write(writer);

        writer.Write(Name);
        writer.Write(Cost);
        writer.Write(ImprovementCost);
        writer.Write(Rents.Count);

        foreach (int rent in Rents)
        {
            writer.Write(rent);
        }
    }

    internal static new StreetSquare Read(BinaryReader reader)
    {
        string name = reader.ReadString();
        int cost = reader.ReadInt32();
        int developmentCost = reader.ReadInt32();
        int length = reader.ReadInt32();
        int[] rents = new int[length];

        for (int i = 0; i < length; i++)
        {
            rents[i] = reader.ReadInt32();
        }

        return new StreetSquare(name, cost, developmentCost, rents);
    }
}
