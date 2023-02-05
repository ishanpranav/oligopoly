using System;
using MessagePack;

namespace Oligopoly.Cards;

[MessagePackObject]
public class ChairmanCard : ICard
{
    public ChairmanCard(string name, int amount)
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
        int amount = controller.Game.Players.Count * Amount;

        controller.Tax(controller.Game.Current, amount);

        if (controller.Game.Current.Cash < 0)
        {
            return;
        }

        foreach (Player player in controller.Game.Players)
        {
            if (player.Id == controller.Game.Current.Id)
            {
                continue;
            }

            controller.Untax(player, amount);
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
