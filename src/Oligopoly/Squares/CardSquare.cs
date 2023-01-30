using System;
using MessagePack;

namespace Oligopoly.Squares;

[MessagePackObject]
public class CardSquare : ISquare
{
    public CardSquare(int deck)
    {
        if (deck < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(deck));
        }

        Deck = deck;
    }

    [Key(0)]
    public int Deck { get; }

    /// <inheritdoc/>
    [IgnoreMember]
    public string Name
    {
        get
        {
            return Deck.ToString();
        }
    }
}
