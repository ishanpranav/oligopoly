using System;
using Oligopoly.Writers;

namespace Oligopoly.Squares;

public class CardSquare : Square
{
    public CardSquare(int deck)
    {
        if (deck < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(deck));
        }

        Deck = deck;
    }

    /// <inheritdoc/>
    public override SquareType Type
    {
        get
        {
            return SquareType.Card;
        }
    }

    public int Deck { get; }

    /// <inheritdoc/>
    public override void Write(Writer writer)
    {
        base.Write(writer);

        writer.Write(Deck);
    }
}
