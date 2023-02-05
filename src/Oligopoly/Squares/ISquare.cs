using System.Text.Json.Serialization;
using MessagePack;

namespace Oligopoly.Squares;

[JsonDerivedType(typeof(EmptySquare), "none")]
[JsonDerivedType(typeof(CardSquare), "card")]
[JsonDerivedType(typeof(JailSquare), "jail")]
[JsonDerivedType(typeof(PoliceSquare), "police")]
[JsonDerivedType(typeof(RailroadSquare), "railroad")]
[JsonDerivedType(typeof(StreetSquare), "street")]
[JsonDerivedType(typeof(TaxSquare), "tax")]
[JsonDerivedType(typeof(UtilitySquare), "utility")]
[Union(0, typeof(EmptySquare))]
[Union(1, typeof(CardSquare))]
[Union(2, typeof(JailSquare))]
[Union(3, typeof(PoliceSquare))]
[Union(4, typeof(RailroadSquare))]
[Union(5, typeof(StreetSquare))]
[Union(6, typeof(TaxSquare))]
[Union(7, typeof(UtilitySquare))]
public interface ISquare
{
    string Name { get; }

    void Advance(Player player, GameController controller);
}
