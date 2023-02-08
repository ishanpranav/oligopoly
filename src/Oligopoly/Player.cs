using System.Collections.Generic;
using System.Text.Json.Serialization;
using MessagePack;
using Oligopoly.Agents;
using Oligopoly.Cards;

namespace Oligopoly;

[MessagePackObject]
public class Player : IAsset
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

    /// <inheritdoc/>
    public int Appraise(Board board, Game game)
    {
        int result = Cash;

        foreach (Deed deed in game.Deeds.Values)
        {
            if (deed.PlayerId == Id)
            {
                result += deed.Appraise(board, game);
            }
        }

        return result;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
