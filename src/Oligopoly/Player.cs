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

    private readonly Queue<CardId> _queue;

    public Player(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
        _queue = new Queue<CardId>();
    }

    [SerializationConstructor]
    public Player(string name, IEnumerable<CardId> cards)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(cards);

        Name = name;
        _queue = new Queue<CardId>(cards);
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
    public IEnumerable<CardId> CardIds
    {
        get
        {
            return _queue;
        }
    }

    [Key(2)]
    public int Cash { get; set; }

    [Key(3)]
    public int SquareId { get; set; } = 1;

    [Key(4)]
    public int Sentence { get; set; }

    public bool TryPlay(out CardId cardId)
    {
        return _queue.TryDequeue(out cardId);
    }

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
