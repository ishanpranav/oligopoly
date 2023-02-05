using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MessagePack;

namespace Oligopoly.Squares;

[MessagePackObject]
public class StreetSquare : PropertySquare
{
    public StreetSquare(string name, int cost, int groupId, IReadOnlyList<int> rents) : base(name)
    {
        Cost = cost;
        GroupId = groupId;
        Rents = rents;
    }

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
    public override int GetRent(Board board, int dice)
    {
        return 0;
    }

    /// <inheritdoc/>
    public override int Appraise(Board board, Game game)
    {
        return Cost;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
