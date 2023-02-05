using System;
using System.Collections.Generic;
using MessagePack;
using Oligopoly.Agents;
using Oligopoly.Cards;

namespace Oligopoly;

[MessagePackObject]
public class Player : IAsset
{
    private Agent? _agent;

    public Player(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
        CardIds = new Queue<CardId>();
    }

    [SerializationConstructor]
    public Player(string name, Queue<CardId> cardIds)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(cardIds);

        Name = name;
        CardIds = cardIds;
    }

    [IgnoreMember]
    public int Id { get; set; }

    [IgnoreMember]
    public Agent Agent
    {
        get
        {
            if (_agent is null)
            {
                return Agent.Default;
            }

            return _agent;
        }
        set
        {
            _agent = value;
        }
    }

    [Key(0)]
    public string Name { get; }

    [Key(1)]
    public Queue<CardId> CardIds { get; }

    [Key(2)]
    public int Cash { get; set; }

    [Key(3)]
    public int SquareId { get; set; } = 1;

    [Key(4)]
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
