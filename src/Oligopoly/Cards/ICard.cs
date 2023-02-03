using System.Text.Json.Serialization;
using MessagePack;

namespace Oligopoly.Cards;

[JsonDerivedType(typeof(JailEscapeCard), "jailEscape")]
[JsonDerivedType(typeof(AdvanceCard), "advance")]
[Union(0, typeof(JailEscapeCard))]
[Union(1, typeof(AdvanceCard))]
public interface ICard
{
    CardId Id { get; set; }
    string Name { get; }
}
