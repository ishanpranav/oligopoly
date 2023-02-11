using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MessagePack;
using Oligopoly.Agents;
using Oligopoly.Assets;

namespace Oligopoly;

[MessagePackObject]
public class Player : IAppraisable, IComparable, IComparable<Player>, IEquatable<Player>
{
    public Player(int id, string name) : this(id, name, new Queue<CardId>()) { }

    [SerializationConstructor]
    public Player(int id, string name, Queue<CardId> cardIds)
    {
        Id = id;
        Name = name;
        CardIds = cardIds;
    }

    [IgnoreMember]
    [JsonIgnore]
    public IAgent Agent { get; set; } = new Agent();

    [Key(0)]
    public int Id { get; }

    [Key(1)]
    public string Name { get; }

    [Key(2)]
    public Queue<CardId> CardIds { get; }

    [Key(3)]
    public int Cash { get; set; }

    [Key(4)]
    public int SquareId { get; set; }

    [Key(5)]
    public int Sentence { get; set; }

    public IEnumerable<IAsset> GetAssets(Game game)
    {
        foreach (Deed deed in game.Deeds.Values)
        {
            if (deed.PlayerId != Id)
            {
                continue;
            }

            yield return deed;
        }

        CardId[] array = new CardId[CardIds.Count];

        CardIds.CopyTo(array, arrayIndex: 0);

        foreach (CardId cardId in array)
        {
            yield return cardId;
        }
    }

    /// <inheritdoc/>
    public int Appraise(Board board, Game game)
    {
        int result = Cash;

        foreach (IAsset asset in GetAssets(game))
        {
            result += asset.Appraise(board, game);
        }

        return result;
    }

    /// <inheritdoc/>
    public int CompareTo(object? obj)
    {
        if (obj is null)
        {
            return 1;
        }

        if (obj is not Player other)
        {
            throw new ArgumentException(message: null, nameof(obj));
        }

        return CompareTo(other);
    }

    /// <inheritdoc/>
    public int CompareTo(Player? other)
    {
        if (other is null)
        {
            return 1;
        }

        return Id - other.Id;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is Player other && Equals(other);
    }

    /// <inheritdoc/>
    public bool Equals(Player? other)
    {
        return other is not null && other.Id == Id;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Id;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
