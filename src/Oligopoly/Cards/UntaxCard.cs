using System.Text.Json.Serialization;
using MessagePack;

namespace Oligopoly.Cards;

[MessagePackObject]
public class UntaxCard : ICard
{
    public UntaxCard(string name, int amount)
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
        controller.Untax(player, Amount);
        controller.Game.Discard(Id);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
