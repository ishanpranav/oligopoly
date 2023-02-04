using System;
using MessagePack;

namespace Oligopoly.Cards;

[MessagePackObject]
public class TaxCard : ICard
{
    public TaxCard(string name, int amount)
    {
        ArgumentNullException.ThrowIfNull(name);

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
