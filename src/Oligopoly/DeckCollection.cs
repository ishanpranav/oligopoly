using System;
using System.Collections.Generic;
using System.Linq;
using MessagePack;
using Nito.Collections;
using Oligopoly.Cards;

namespace Oligopoly;

[MessagePackObject]
public class DeckCollection
{
    private readonly Deque<int>[] _deques;

    [SerializationConstructor]
    public DeckCollection(IReadOnlyList<IEnumerable<int>> decks)
    {
        _deques = new Deque<int>[decks.Count];

        for (int i = 0; i < decks.Count; i++)
        {
            _deques[i] = new Deque<int>(decks[i]);
        }
    }

    public DeckCollection(IReadOnlyList<Deck> decks)
    {
        _deques = new Deque<int>[decks.Count];

        for (int i = 0; i < decks.Count; i++)
        {
            _deques[i] = new Deque<int>(decks[i].Cards.Select(x => x.Id));
        }
    }

    public void Shuffle()
    {
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

    public Card Draw(Deck deck)
    {
        return deck.Cards[_deques[deck.Id].RemoveFromFront()];
    }

    public void Discard(Card card)
    {
        _deques[card.DeckId].AddToBack(card.Id);
    }
}
