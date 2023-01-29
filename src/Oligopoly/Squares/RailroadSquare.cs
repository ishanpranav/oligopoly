using System.Collections.Generic;

namespace Oligopoly.Squares;

public class RailroadSquare : PropertySquare
{
    public RailroadSquare(string name, IReadOnlyList<int> rents, Group group) : base(name, rents, group) { }

    /// <inheritdoc/>
    public override SquareType Type
    {
        get
        {
            return SquareType.Railroad;
        }
    }

    /// <inheritdoc/>
    public override int Cost
    {
        get
        {
            return Group.ImprovementCost;
        }
    }
}
