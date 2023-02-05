using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using MessagePack;
using Nito.Collections;
using Oligopoly.Cards;
using Oligopoly.Squares;

namespace Oligopoly;

[MessagePackObject]
public class Game
{
    private readonly Deque<int>[] _deques;

    public Game(ICollection<Player> players, IReadOnlyList<ISquare> squares, IReadOnlyList<Deck> decks)
    {
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
    public Game(ICollection<Player> players, IReadOnlyDictionary<int, Deed> deeds, IReadOnlyList<IEnumerable<int>> deckIds)
    {
        Players = players;

        foreach (KeyValuePair<int, Deed> indexedDeed in deeds)
        {
            indexedDeed.Value.SquareId = indexedDeed.Key + 1;
        }

        Deeds = deeds;
        _deques = new Deque<int>[deckIds.Count];

        for (int i = 0; i < deckIds.Count; i++)
        {
            _deques[i] = new Deque<int>(deckIds[i]);
        }
    }

    [IgnoreMember]
    [JsonIgnore]
    public Random Random { get; } = new Random(0);

    [Key(0)]
    public ICollection<Player> Players { get; }

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

    public ICard Draw(Deck deck)
    {
        return deck.Cards[_deques[deck.Id - 1].RemoveFromFront() - 1];
    }

    public void Discard(CardId card)
    {
        _deques[card.DeckId - 1].AddToBack(card.Id);
    }
}
