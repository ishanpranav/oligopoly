using System;
using System.Collections.Generic;
using MessagePack;

namespace Oligopoly.Squares;

[MessagePackObject]
public class StreetSquare : ISquare
{
    public StreetSquare(string name, int cost, int group, IReadOnlyList<int> rents)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (cost <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cost));
        }

        Name = name;
        Cost = cost;
        Group = group;
        Rents = rents;
    }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    [Key(1)]
    public int Cost { get; }

    [Key(2)]
    public int Group { get; }

    [Key(3)]
    public IReadOnlyList<int> Rents { get; }
}
