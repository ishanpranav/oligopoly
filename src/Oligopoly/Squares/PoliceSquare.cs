namespace Oligopoly.Squares;

internal sealed class PoliceSquare : Square
{
    /// <inheritdoc/>
    public override SquareType Type
    {
        get
        {
            return SquareType.Police;
        }
    }
}
