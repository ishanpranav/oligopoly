using System;
using MessagePack;

namespace Oligopoly.Squares;

[MessagePackObject]
public class RailroadSquare : IAsset, ISquare
{
    public RailroadSquare(string name)
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
        return board.RailroadCost;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
