namespace Oligopoly.Squares;

internal sealed class JailSquare : Square
{
    public JailSquare() { }

    /// <inheritdoc/>
    public override SquareType Type
    {
        get
        {
            return SquareType.Jail;
        }
    }
}
