using System;
using MessagePack;

namespace Oligopoly.Squares;

[MessagePackObject]
public class UtilitySquare : IPropertySquare
{
    public UtilitySquare(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name; 
    }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    /// <inheritdoc/>
    public int Appraise(Board board, Game game)
    {
        return board.UtilityCost;
    }

    /// <inheritdoc/>
    public int GetRent(Board board, Roll roll)
    {
        return 0;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
