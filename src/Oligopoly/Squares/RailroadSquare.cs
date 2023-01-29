using System.Collections.Generic;

namespace Oligopoly.Squares;

public class RailroadSquare : PropertySquare
{
    public RailroadSquare(string name, int cost, IReadOnlyList<int> rents) : base(name, cost, rents) { }

    /// <inheritdoc/>
    public override SquareType Type
    {
        get
        {
            return SquareType.Railroad;
        }
    }
}
