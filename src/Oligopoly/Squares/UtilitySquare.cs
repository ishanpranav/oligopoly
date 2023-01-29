using System.Collections.Generic;

namespace Oligopoly.Squares;

public class UtilitySquare : PropertySquare
{
    public UtilitySquare(string name, IReadOnlyList<int> rents, Group group) : base(name, rents, group) { }

    /// <inheritdoc/>
    public override SquareType Type
    {
        get
        {
            return SquareType.Utility;
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
