using System.IO;

namespace Oligopoly.Squares;

public class CardSquare : Square
{
    public CardSquare() { }

    /// <inheritdoc/>
    public override SquareType Type
    {
        get
        {
            return SquareType.Card;
        }
    }
}
