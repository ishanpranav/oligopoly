using System;
using MessagePack;

namespace Oligopoly.Cards;

[MessagePackObject]
public class TravelCard : ICard
{
    public TravelCard(string name, int distance)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
        Distance = distance;
    }

    /// <inheritdoc/>
    [IgnoreMember]
    public CardId Id { get; set; }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    [Key(1)]
    public int Distance { get; }

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
