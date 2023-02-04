using System;
using MessagePack;

namespace Oligopoly.Cards;

[MessagePackObject]
public class BonusCard : ICard
{
    public BonusCard(string name, int amount)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }

        Name = name;
        Amount = amount;
    }

    /// <inheritdoc/>
    [IgnoreMember]
    public CardId Id { get; set; }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    [Key(1)]
    public int Amount { get; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
