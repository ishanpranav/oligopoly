using System;
using System.Text.Json.Serialization;
using MessagePack;

namespace Oligopoly.Dice;

[MessagePackObject]
public class D6PairDice : IDice
{
    private readonly Random _random;

    public D6PairDice()
    {
        _random = Random.Shared;
    }

    public D6PairDice(Random random)
    {
        _random = random;
    }

    /// <inheritdoc/>
    [IgnoreMember]
    [JsonIgnore]
    public int Amount { get; private set; }

    /// <inheritdoc/>
    public bool Roll(GameController controller, Player player)
    {
        int first = _random.Next(minValue: 1, maxValue: 7);
        int second = _random.Next(minValue: 1, maxValue: 7);

        Amount = first + second;

        Console.WriteLine("Rolled ({0}, {1})", first, second);

        if (player.Sentence > 0)
        {
            if (first == second)
            {
                player.Sentence = 0;

                controller.Jump(player, Amount);

                return false;
            }

            player.Sentence--;

            if (player.Sentence > 0)
            {
                return false;
            }

            controller.Tax(player, controller.Board.Bail);
        }

        controller.Jump(player, Amount);

        return first == second && player.Sentence is 0;
    }
}
