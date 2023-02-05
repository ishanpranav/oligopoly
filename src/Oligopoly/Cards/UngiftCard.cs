using System;
using MessagePack;

namespace Oligopoly.Cards;

[MessagePackObject]
public class UngiftCard : ICard
{
    public UngiftCard(string name, int amount)
    {
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
    public void Draw(Player player, GameController controller)
    {
        Console.WriteLine("It is {0}'s birthday", player);

        foreach (Player other in controller.Game.Players)
        {
            if (other.Id == player.Id)
            {
                continue;
            }

            controller.Demand(other, player, Amount);
        }
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
