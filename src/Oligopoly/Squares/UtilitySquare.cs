using System;
using System.Collections.Generic;

namespace Oligopoly.Squares;

public class UtilitySquare : PropertySquare
{
    public UtilitySquare(string name, int cost, IReadOnlyList<int> rents) : base(name, cost, rents) { }

    /// <inheritdoc/>
    public override SquareType Type
    {
        get
        {
            return SquareType.Utility;
        }
    }
}
