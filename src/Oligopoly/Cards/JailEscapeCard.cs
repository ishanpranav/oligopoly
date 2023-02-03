using System;
using MessagePack;

namespace Oligopoly.Cards;

[MessagePackObject]
public class JailEscapeCard : ICard
{
    public JailEscapeCard(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }

    [IgnoreMember]
    public CardId Id { get; set; }

    [Key(0)]
    public string Name { get; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
