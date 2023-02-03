using System;
using System.Text.Json.Serialization;
using MessagePack;

namespace Oligopoly.Cards;

[JsonDerivedType(typeof(JailEscapeCard), "jailEscape")]
[Union(0, typeof(JailEscapeCard))]
public abstract class Card
{
    public Card(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }

    [IgnoreMember]
    public int Id { get; set; }

    [IgnoreMember]
    public int DeckId { get; set; }

    [Key(0)]
    public string Name { get; }

    public virtual void Draw(Player player)
    {
        Play(player);
    }

    public abstract void Play(Player player);
}
