using System.Text.Json.Serialization;
using MessagePack;

namespace Oligopoly.Cards;

[MessagePackObject]
public class AdvanceCard : ICard
{
    public AdvanceCard(string name, int destinationId)
    {
        Name = name;
        SquareId = destinationId;
    }

    /// <inheritdoc/>
    [IgnoreMember]
    public CardId Id { get; set; }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    [JsonPropertyName("square")]
    [Key(1)]
    public int SquareId { get; }

    /// <inheritdoc/>
    public void Draw(Player player, GameController controller)
    {
        if (player.SquareId > SquareId)
        {
            controller.Untax(player, controller.Board.Salary);
        }

        controller.Advance(player, SquareId);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
