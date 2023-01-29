using System.Collections.Generic;
using Oligopoly.Writers;

namespace Oligopoly.Squares;

public class StreetSquare : PropertySquare
{
    public StreetSquare(string name, int cost, IReadOnlyList<int> rents) : base(name, cost, rents) { }

    /// <inheritdoc/>
    public override SquareType Type
    {
        get
        {
            return SquareType.Street;
        }
    }

    /// <inheritdoc/>
    public override void Write(Writer writer)
    {
        base.Write(writer);

        writer.Write(Cost);

        foreach (int rent in Rents)
        {
            writer.Write(rent);
        }
    }
}
