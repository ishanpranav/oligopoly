using System.Text.Json.Serialization;
using MessagePack;

namespace Oligopoly.Cards;

[JsonDerivedType(typeof(JailbreakCard), "jailbreak")]
[JsonDerivedType(typeof(PoliceCard), "police")]
[JsonDerivedType(typeof(RailroadCard), "railroad")]
[JsonDerivedType(typeof(AdvanceCard), "advance")]
[JsonDerivedType(typeof(TravelCard), "travel")]
[JsonDerivedType(typeof(RepairCard), "repair")]
[JsonDerivedType(typeof(TaxCard), "tax")]
[Union(0, typeof(JailbreakCard))]
[Union(1, typeof(PoliceCard))]
[Union(2, typeof(RailroadCard))]
[Union(3, typeof(AdvanceCard))]
[Union(4, typeof(TravelCard))]
[Union(5, typeof(RepairCard))]
[Union(6, typeof(TaxCard))]
public interface ICard
{
    CardId Id { get; set; }
    string Name { get; }
}
