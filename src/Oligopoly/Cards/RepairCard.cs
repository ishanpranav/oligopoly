using System;
using MessagePack;

namespace Oligopoly.Cards;

[MessagePackObject]
public class RepairCard : ICard
{
    public RepairCard(string name, int houseCost, int hotelCost)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (houseCost < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(houseCost));
        }

        if (hotelCost < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(hotelCost));
        }

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
    public void Draw(GameController controller)
    {

    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
