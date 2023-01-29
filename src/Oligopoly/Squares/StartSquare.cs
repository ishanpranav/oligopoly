using System.IO;

namespace Oligopoly.Squares;

public class StartSquare : Square
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

    internal static new StartSquare Read(BinaryReader reader)
    {
        return new StartSquare();
    }
}
