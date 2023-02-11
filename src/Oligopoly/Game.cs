using System.Collections.Generic;
using System.Linq;
using MessagePack;
using Nito.Collections;
using Oligopoly.Assets;
using Oligopoly.Cards;
using Oligopoly.Dice;
using Oligopoly.Shuffles;
using Oligopoly.Squares;

namespace Oligopoly;

[MessagePackObject]
public class Game
{
    private readonly Deque<int>[] _deques;

    public Game(IReadOnlyList<ISquare> squares, IReadOnlyList<Deck> decks, IDice dice, IShuffle shuffle)
    {
        Players = new HashSet<Player>();

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
            shuffle.Shuffle(deque);
        }

        Dice = dice;
    }

    [SerializationConstructor]
    public Game(ICollection<Player> players, IReadOnlyDictionary<int, Deed> deeds, IReadOnlyList<IEnumerable<int>> deckIds, IDice dice)
    {
        Players = new SortedSet<Player>(players);

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

        Dice = dice;
    }

    [Key(0)]
    public ICollection<Player> Players { get; }

    [Key(1)]
    public IReadOnlyDictionary<int, Deed> Deeds { get; }

    [Key(2)]
    public IReadOnlyList<IReadOnlyList<int>> DeckIds
    {
        get
        {
            return _deques;
        }
    }

    [Key(3)]
    public IDice Dice { get; }

    [Key(4)]
    public int Houses { get; set; }

    [Key(5)]
    public int Hotels { get; set; }

    public ICard Draw(Deck deck)
    {
        return deck.Cards[_deques[deck.Id - 1].RemoveFromFront() - 1];
    }

    public void Discard(CardId card)
    {
        _deques[card.DeckId - 1].AddToBack(card.Id);
    }
}
