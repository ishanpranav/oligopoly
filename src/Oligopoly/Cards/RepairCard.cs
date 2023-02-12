using System.Collections.Generic;
using System.Text.Json.Serialization;
using MessagePack;
using Oligopoly.Assets;
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
    [JsonIgnore]
    public CardId Id { get; set; }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    [Key(1)]
    public int HouseCost { get; }

    [Key(2)]
    public int HotelCost { get; }

    /// <inheritdoc/>
    public void Draw(Player player, Controller controller)
    {
        int cost = 0;

        foreach (KeyValuePair<int, Deed> indexedDeed in controller.Game.Deeds)
        {
            if (indexedDeed.Value.PlayerId != player.Id)
            {
                continue;
            }

            if (indexedDeed.Value.Improvements is 0)
            {
                continue;
            }

            if (controller.Board.Squares[indexedDeed.Key] is not StreetSquare streetSquare)
            {
                continue;
            }

            if (indexedDeed.Value.Improvements == streetSquare.Rents.Count - 1)
            {
                cost += HotelCost;
            }
            else
            {
                cost += HouseCost * indexedDeed.Value.Improvements;
            }
        }

        controller.Tax(player, cost);
        controller.Game.Discard(Id);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
