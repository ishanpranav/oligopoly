using System;
using MessagePack;

namespace Oligopoly.Squares;

[MessagePackObject]
public class UtilitySquare : PropertySquare
{
    public UtilitySquare(string name) : base(name) { }

    /// <inheritdoc/>
    public override int GetRent(Board board, int dice)
    {
        return 0;
    }

    /// <inheritdoc/>
    public override int Appraise(Board board, Game game)
    {
        return board.UtilityCost;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
