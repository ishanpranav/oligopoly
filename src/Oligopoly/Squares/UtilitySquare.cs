using System;
using MessagePack;

namespace Oligopoly.Squares;

[MessagePackObject]
public class UtilitySquare : IAsset, ISquare
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
    public int Appraise(Board board)
    {
        return board.UtilityCost;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
