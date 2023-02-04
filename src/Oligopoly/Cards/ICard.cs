using System.Text.Json.Serialization;
using MessagePack;

namespace Oligopoly.Cards;

[JsonDerivedType(typeof(JailEscapeCard), "jailEscape")]
[JsonDerivedType(typeof(PoliceCard), "police")]
[JsonDerivedType(typeof(AdvanceCard), "advance")]
[JsonDerivedType(typeof(BonusCard), "bonus")]
[JsonDerivedType(typeof(RepairCard), "repair")]
[Union(0, typeof(JailEscapeCard))]
[Union(1, typeof(PoliceCard))]
[Union(2, typeof(AdvanceCard))]
[Union(3, typeof(BonusCard))]
[Union(4, typeof(RepairCard))]
public interface ICard
{
    CardId Id { get; set; }
    string Name { get; }
}
