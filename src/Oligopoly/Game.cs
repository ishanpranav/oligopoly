using System;
using System.Collections.Generic;
using System.Linq;
using MessagePack;
using Nito.Collections;
using Oligopoly.Cards;
using Oligopoly.Squares;

namespace Oligopoly;

[MessagePackObject]
public class Game
{
    private readonly Deque<int>[] _deques;

    private int _turn;

    public Game(IReadOnlyList<Player> players, IReadOnlyList<ISquare> squares, IReadOnlyList<Deck> decks)
    {
        ArgumentNullException.ThrowIfNull(players);
        ArgumentNullException.ThrowIfNull(squares);
        ArgumentNullException.ThrowIfNull(decks);

        for (int i = 0; i < players.Count; i++)
        {
            players[i].Id = i + 1;
        }

        Players = players;

        Dictionary<int, Deed> dictionary = new Dictionary<int, Deed>();

        for (int i = 0; i < squares.Count; i++)
        {
            if (squares[i] is IPropertySquare)
            {
                dictionary[i] = new Deed()
                {
                    SquareId = i + 1
                };
            }
        }

        Deeds = dictionary;
        _deques = new Deque<int>[decks.Count];

        for (int i = 0; i < decks.Count; i++)
        {
            _deques[i] = new Deque<int>(decks[i].Cards.Select(x => x.Id.Id));
        }

        foreach (Deque<int> deque in _deques)
        {
            int n = deque.Count;

            while (n > 1)
            {
                n--;

                int k = Random.Shared.Next(n + 1);
                int id = deque[k];

                deque[k] = deque[n];
                deque[n] = id;
            }
        }
    }

    [SerializationConstructor]
    public Game(IReadOnlyList<Player> players, IReadOnlyDictionary<int, Deed> deedIds, IReadOnlyList<IEnumerable<int>> deckIds)
    {
        ArgumentNullException.ThrowIfNull(players);
        ArgumentNullException.ThrowIfNull(deedIds);
        ArgumentNullException.ThrowIfNull(deckIds);

        for (int i = 0; i < players.Count; i++)
        {
            players[i].Id = i + 1;
        }

        Players = players;

        foreach (KeyValuePair<int, Deed> deedId in deedIds)
        {
            deedId.Value.SquareId = deedId.Key + 1;
        }

        Deeds = deedIds;
        _deques = new Deque<int>[deckIds.Count];

        for (int i = 0; i < deckIds.Count; i++)
        {
            _deques[i] = new Deque<int>(deckIds[i]);
        }
    }

    [IgnoreMember]
    public Player Current
    {
        get
        {
            int i = Turn % Players.Count;

            if (i >= Players.Count)
            {
                throw new InvalidOperationException();
            }

            return Players[i];
        }
    }

    [Key(0)]
    public IReadOnlyList<Player> Players { get; }

    [Key(1)]
    public IReadOnlyDictionary<int, Deed> Deeds { get; }

    [Key(2)]
    public IReadOnlyList<IEnumerable<int>> DeckIds
    {
        get
        {
            return _deques;
        }
    }

    [Key(3)]
    public int Turn
    {
        get
        {
            return _turn;
        }
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            _turn = value;
        }
    }

    public void Discard(CardId card)
    {
        _deques[card.DeckId - 1].AddToBack(card.Id);
    }

    //public event EventHandler? TurnEnded;   // Player
    //public event EventHandler? Moved; // Player
    //public event EventHandler? JailEscapeAcquired;
    //public event EventHandler? AuctionSucceded; // Property, Player (winner), Amount (bid)
    //public event EventHandler? AuctionFailed; // Property
    //public event EventHandler? DealResponded; // Outcome
    //public event EventHandler? DealClosed; // Deal
    //public event EventHandler? Bankrupted; // Player
    //public event EventHandler? Ended; // Player (winner), Amount (rounds)
    //public event EventHandler? Error; // Message
}
