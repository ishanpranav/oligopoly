using System.Text.Json.Serialization;
using MessagePack;
using Oligopoly.Assets;
using Oligopoly.Squares;

namespace Oligopoly.Cards;

public abstract class FlyCard : ICard
{
    protected FlyCard(string name)
    {
        Name = name;
    }

    /// <inheritdoc/>
    [IgnoreMember]
    [JsonIgnore]
    public CardId Id { get; set; }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    /// <inheritdoc/>
    public void Draw(Player player, Controller controller)
    {
        for (int squareId = player.SquareId + 1; squareId <= controller.Board.Squares.Count; squareId++)
        {
            if (CanLand(controller.Board.Squares[squareId - 1]))
            {
                controller.Fly(player, squareId);
                controller.Game.Discard(Id);

                return;
            }
        }

        for (int squareId = 1; squareId < player.SquareId; squareId++)
        {
            if (CanLand(controller.Board.Squares[squareId - 1]))
            {
                controller.Untax(player, controller.Board.Salary);
                controller.Fly(player, squareId);
                controller.Game.Discard(Id);

                return;
            }
        }
    }

    protected abstract bool CanLand(ISquare square);

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
