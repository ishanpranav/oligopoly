using MessagePack;

namespace Oligopoly.Dice;

[Union(0, typeof(D6PairDice))]
public interface IDice
{
    int Amount { get; }

    bool Roll(Controller controller, Player player);
}
