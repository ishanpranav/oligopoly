using System.Text.Json.Serialization;
using MessagePack;
using Oligopoly.Assets;

namespace Oligopoly.Cards;

[JsonDerivedType(typeof(JailbreakCard), "jailbreak")]
[JsonDerivedType(typeof(PoliceCard), "police")]
[JsonDerivedType(typeof(RailroadCard), "railroad")]
[JsonDerivedType(typeof(AdvanceCard), "advance")]
[JsonDerivedType(typeof(JumpCard), "jump")]
[JsonDerivedType(typeof(RepairCard), "repair")]
[JsonDerivedType(typeof(UtilityCard), "utility")]
[JsonDerivedType(typeof(TaxCard), "tax")]
[JsonDerivedType(typeof(UntaxCard), "untax")]
[JsonDerivedType(typeof(GiftCard), "gift")]
[JsonDerivedType(typeof(UngiftCard), "ungift")]
[Union(0, typeof(JailbreakCard))]
[Union(1, typeof(PoliceCard))]
[Union(2, typeof(RailroadCard))]
[Union(3, typeof(AdvanceCard))]
[Union(4, typeof(JumpCard))]
[Union(5, typeof(RepairCard))]
[Union(6, typeof(UtilityCard))]
[Union(7, typeof(TaxCard))]
[Union(8, typeof(UntaxCard))]
[Union(9, typeof(GiftCard))]
[Union(10, typeof(UngiftCard))]
public interface ICard
{
    CardId Id { get; set; }
    string Name { get; }

    void Draw(Player player, GameController controller);
}
