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
    private readonly HashSet<int> _hashSet;

    public Player(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
        _queue = new Queue<CardId>();
        _hashSet = new HashSet<int>();
    }

    [SerializationConstructor]
    public Player(string name, IEnumerable<CardId> cards, IReadOnlySet<int> deeds)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(cards);
        ArgumentNullException.ThrowIfNull(deeds);

        Name = name;
        _queue = new Queue<CardId>(cards);
        _hashSet = new HashSet<int>(deeds);
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
    public IReadOnlySet<int> DeedIds
    {
        get
        {
            return _hashSet;
        }
    }

    [Key(3)]
    public int Cash { get; set; }

    [Key(4)]
    public int JailTurns { get; set; }

    public bool TryPlay(out CardId cardId)
    {
        return _queue.TryDequeue(out cardId);
    }

    /// <inheritdoc/>
    public int Appraise(Board board)
    {
        int result = Cash;

        foreach (int deedId in _hashSet)
        {
            result += board.Appraise(deedId);
        }

        return result;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
