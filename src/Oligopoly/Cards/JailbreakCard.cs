using System;
using MessagePack;

namespace Oligopoly.Cards;

[MessagePackObject]
public class JailbreakCard : ICard
{
    public JailbreakCard(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

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
        player.CardIds.Enqueue(Id);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
