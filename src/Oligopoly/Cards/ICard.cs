using System.Text.Json.Serialization;
using MessagePack;

namespace Oligopoly.Cards;

[JsonDerivedType(typeof(JailEscapeCard), "jailEscape")]
[JsonDerivedType(typeof(AdvanceCard), "advance")]
[JsonDerivedType(typeof(RepairCard), "repair")]
[Union(0, typeof(JailEscapeCard))]
[Union(1, typeof(AdvanceCard))]
[Union(2, typeof(RepairCard))]
public interface ICard
{
    CardId Id { get; set; }
    string Name { get; }
}
