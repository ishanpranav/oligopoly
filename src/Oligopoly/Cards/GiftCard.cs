using System.Text.Json.Serialization;
using MessagePack;
using Oligopoly.Assets;

namespace Oligopoly.Cards;

[MessagePackObject]
public class GiftCard : ICard
{
    public GiftCard(string name, int amount)
    {
        Name = name;
        Amount = amount;
    }

    /// <inheritdoc/>
    [IgnoreMember]
    [JsonIgnore]
    public CardId Id { get; set; }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    [Key(1)]
    public int Amount { get; }

    /// <inheritdoc/>
    public void Draw(Player player, GameController controller)
    {
        int amount = (controller.Game.Players.Count - 1) * Amount;

        controller.Tax(player, amount);

        if (player.Cash < 0)
        {
            return;
        }

        foreach (Player other in controller.Game.Players)
        {
            if (other.Id == player.Id)
            {
                continue;
            }

            controller.Untax(other, Amount);
        }

        controller.Game.Discard(Id);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
