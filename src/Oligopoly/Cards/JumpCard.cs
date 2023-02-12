using System.Text.Json.Serialization;
using MessagePack;
using Oligopoly.Assets;

namespace Oligopoly.Cards;

[MessagePackObject]
public class JumpCard : ICard
{
    public JumpCard(string name, int distance)
    {
        Name = name;
        Distance = distance;
    }

    /// <inheritdoc/>
    [IgnoreMember]
    [JsonIgnore]
    public CardId Id { get; set; }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    [Key(1)]
    public int Distance { get; }

    /// <inheritdoc/>
    public void Draw(Player player, Controller controller)
    {
        controller.Jump(player, Distance);
        controller.Game.Discard(Id);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
