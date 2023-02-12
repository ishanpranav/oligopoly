using System.Text.Json.Serialization;
using MessagePack;
using Oligopoly.Assets;

namespace Oligopoly.Cards;

[MessagePackObject]
public class AdvanceCard : ICard
{
    public AdvanceCard(string name, int squareId)
    {
        Name = name;
        SquareId = squareId;
    }

    /// <inheritdoc/>
    [IgnoreMember]
    [JsonIgnore]
    public CardId Id { get; set; }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    [JsonPropertyName("square")]
    [Key(1)]
    public int SquareId { get; }

    /// <inheritdoc/>
    public void Draw(Player player, Controller controller)
    {
        if (player.SquareId > SquareId)
        {
            controller.Untax(player, controller.Board.Salary);
        }

        controller.Advance(player, SquareId);
        controller.Game.Discard(Id);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
