using MessagePack;
using Oligopoly.Squares;

namespace Oligopoly.Cards;

[MessagePackObject]
public class RepairCard : ICard
{
    public RepairCard(string name, int houseCost, int hotelCost)
    {
        Name = name;
        HouseCost = houseCost;
        HotelCost = hotelCost;
    }

    /// <inheritdoc/>
    [IgnoreMember]
    public CardId Id { get; set; }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    [Key(1)]
    public int HouseCost { get; }

    [Key(2)]
    public int HotelCost { get; }

    /// <inheritdoc/>
    public void Draw(Player player, GameController controller)
    {
        int cost = 0;

        foreach (Deed deed in controller.Game.Deeds.Values)
        {
            if (deed.PlayerId != player.Id)
            {
                continue;
            }

            if (deed.Improvements is 0)
            {
                continue;
            }

            if (controller.Board.Squares[deed.SquareId - 1] is not StreetSquare streetSquare)
            {
                continue;
            }

            if (deed.Improvements == streetSquare.Rents.Count - 1)
            {
                cost += HotelCost;
            }
            else
            {
                cost += HouseCost;
            }
        }

        controller.Tax(player, cost);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
