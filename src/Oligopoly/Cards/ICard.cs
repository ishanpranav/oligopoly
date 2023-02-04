using System.Text.Json.Serialization;
using MessagePack;

namespace Oligopoly.Cards;

[JsonDerivedType(typeof(JailEscapeCard), "jailEscape")]
[JsonDerivedType(typeof(PoliceCard), "police")]
[JsonDerivedType(typeof(AdvanceCard), "advance")]
[JsonDerivedType(typeof(TravelCard), "travel")]
[JsonDerivedType(typeof(RepairCard), "repair")]
[JsonDerivedType(typeof(TaxCard), "tax")]
[Union(0, typeof(JailEscapeCard))]
[Union(1, typeof(PoliceCard))]
[Union(2, typeof(AdvanceCard))]
[Union(3, typeof(TravelCard))]
[Union(4, typeof(RepairCard))]
[Union(5, typeof(TaxCard))]
public interface ICard
{
    CardId Id { get; set; }
    string Name { get; }
}
