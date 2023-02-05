﻿using System;
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

    public Game(IReadOnlyList<Player> players, IReadOnlyList<ISquare> squares, IReadOnlyList<Deck> decks)
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].Id = i + 1;
        }

        Players = players;

        Dictionary<int, Deed> dictionary = new Dictionary<int, Deed>();

        for (int i = 0; i < squares.Count; i++)
        {
            if (squares[i] is PropertySquare propertySquare)
            {
                Deed deed = new Deed()
                {
                    SquareId = i + 1
                };

                dictionary[i] = deed;
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

                int k = Random.Next(n + 1);
                int id = deque[k];

                deque[k] = deque[n];
                deque[n] = id;
            }
        }
    }

    [SerializationConstructor]
    public Game(IReadOnlyList<Player> players, IReadOnlyDictionary<int, Deed> deedIds, IReadOnlyList<IEnumerable<int>> deckIds)
    {
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
    public Random Random { get; } = new Random(0);

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
    public int Turn { get; set; }

    public ICard Draw(Deck deck)
    {
        return deck.Cards[_deques[deck.Id - 1].RemoveFromFront() - 1];
    }

    public void Discard(CardId card)
    {
        _deques[card.DeckId - 1].AddToBack(card.Id);
    }
}
