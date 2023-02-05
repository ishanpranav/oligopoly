using System;
using MessagePack;

namespace Oligopoly.Cards;

[MessagePackObject]
public class UtilityCard : ICard
{
    public UtilityCard(string name)
    {
        Name = name;
    }

    /// <inheritdoc/>
    [IgnoreMember]
    public CardId Id { get; set; }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    /// <inheritdoc/>
    public void Draw(Player player, GameController controller)
    {

    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
