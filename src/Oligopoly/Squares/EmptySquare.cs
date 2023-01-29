namespace Oligopoly.Squares;

internal sealed class EmptySquare : Square
{
    /// <inheritdoc/>
    public override SquareType Type
    {
        get
        {
            return SquareType.None;
        }
    }
}
