﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MessagePack;

namespace Oligopoly.Squares;

[MessagePackObject]
public class StreetSquare : IPropertySquare
{
    public StreetSquare(string name, int cost, int groupId, IReadOnlyList<int> rents)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (cost <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cost));
        }

        if (groupId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(groupId));
        }

        ArgumentNullException.ThrowIfNull(rents);

        Name = name;
        Cost = cost;
        GroupId = groupId;
        Rents = rents;
    }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    [Key(1)]
    public int Cost { get; }

    [JsonPropertyName("group")]
    [Key(2)]
    public int GroupId { get; }

    [IgnoreMember]
    [JsonIgnore]
    public Group? Group { get; set; }

    [Key(3)]
    public IReadOnlyList<int> Rents { get; }

    /// <inheritdoc/>
    public int Appraise(Board board, Game game)
    {
        return Cost;
    }

    /// <inheritdoc/>
    public void Land(Player player) { }

    /// <inheritdoc/>
    public int GetRent(Board board, int roll)
    {
        return 0;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
