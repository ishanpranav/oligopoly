using System;
using System.Collections.Generic;
using MessagePack;
using Oligopoly.Cards;

namespace Oligopoly;

[MessagePackObject]
public class Deck
{
    public Deck(string name, IReadOnlyList<ICard> cards)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(cards);

        Name = name;
        Cards = cards;
    }

    [Key(0)]
    public string Name { get; }

    [Key(1)]
    public IReadOnlyList<ICard> Cards { get; }

    [IgnoreMember]
    public int Id { get; set; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
