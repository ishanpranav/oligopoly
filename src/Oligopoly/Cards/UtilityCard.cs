using MessagePack;
using Oligopoly.Squares;

namespace Oligopoly.Cards;

[MessagePackObject]
public class UtilityCard : FlyCard
{
    public UtilityCard(string name) : base(name) { }

    /// <inheritdoc/>
    protected override bool CanLand(ISquare square)
    {
        return square is UtilitySquare;
    }
}
