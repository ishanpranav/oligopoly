using System;
using System.Collections.Generic;
using MessagePack;
using Oligopoly.Agents;
using Oligopoly.Assets;
using Oligopoly.Cards;

namespace Oligopoly;

[MessagePackObject]
public class Player : IAsset
{
    private Agent? _agent;

    private readonly Queue<Card> _queue;

    public Player(int id, string name)
    {
        if (id < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }

        ArgumentNullException.ThrowIfNull(name);

        Id = id;
        Name = name;
        Deeds = Array.Empty<Deed>();
        _queue = new Queue<Card>();
    }

    [SerializationConstructor]
    public Player(int id, string name, IEnumerable<Card> cards)
    {
        if (id < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }

        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(cards);

        Id = id;
        Name = name;
        Deeds = Array.Empty<Deed>();
        _queue = new Queue<Card>(cards);
    }

    [Key(0)]
    public int Id { get; }

    [Key(1)]
    public string Name { get; }

    [Key(2)]
    public int Cash { get; set; }

    [Key(3)]
    public IEnumerable<Card> Cards
    {
        get
        {
            return _queue;
        }
    }

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

    [Key(4)]
    public IReadOnlyCollection<Deed> Deeds { get; }

    [Key(5)]
    public int JailTurns { get; set; }

    public void Play(DeckCollection decks)
    {
        if (_queue.TryDequeue(out Card? card))
        {
            card.Play(player: this);
            decks.Discard(card);
        }
    }

    /// <inheritdoc/>
    public int Appraise()
    {
        int result = Cash;

        foreach (Deed deed in Deeds)
        {
            result += deed.Appraise();
        }

        return result;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
