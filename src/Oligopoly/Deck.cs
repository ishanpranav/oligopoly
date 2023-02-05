using System.Collections.Generic;
using MessagePack;
using Oligopoly.Cards;

namespace Oligopoly;

[MessagePackObject]
public class Deck
{
    public Deck(string name, IReadOnlyList<ICard> cards)
    {
        Name = name;
        Cards = cards;
    }

    [IgnoreMember]
    public int Id { get; set; }

    [Key(0)]
    public string Name { get; }

    [Key(1)]
    public IReadOnlyList<ICard> Cards { get; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
