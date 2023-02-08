using MessagePack;

namespace Oligopoly.Dice;

[Union(0, typeof(D6PairDice))]
public interface IDice
{
    int Amount { get; }

    bool Roll(GameController controller, Player player);
}
