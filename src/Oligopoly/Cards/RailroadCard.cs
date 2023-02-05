using MessagePack;
using Oligopoly.Squares;

namespace Oligopoly.Cards;

[MessagePackObject]
public class RailroadCard : FlyCard
{
    public RailroadCard(string name) : base(name) { }

    /// <inheritdoc/>
    protected override bool CanLand(ISquare square)
    {
        return square is RailroadSquare;
    }
}
