using System;
using MessagePack;

namespace Oligopoly.Cards;

public class GiftCard : ICard
{
    public GiftCard(string name, int amount)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }

        Name = name;
        Amount = amount;
    }

    /// <inheritdoc/>
    [IgnoreMember]
    public CardId Id { get; set; }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    [Key(1)]
    public int Amount { get; }

    /// <inheritdoc/>
    public void Draw(GameController controller)
    {
        Console.WriteLine("It is {0}'s birthday", controller.Game.Current);

        foreach (Player player in controller.Game.Players)
        {
            if (player.Id == controller.Game.Current.Id)
            {
                continue;
            }

            controller.Transfer(player, controller.Game.Current, Amount);
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
