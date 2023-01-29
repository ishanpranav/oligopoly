using System.IO;

namespace Oligopoly.Squares;

internal sealed class StartSquare : Square
{
    public StartSquare() { }

    /// <inheritdoc/>
    public override SquareType Type
    {
        get
        {
            return SquareType.Start;
        }
    }
}
