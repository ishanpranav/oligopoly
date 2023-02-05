using MessagePack;
using Oligopoly.Squares;

namespace Oligopoly;

[Union(0, typeof(StreetSquare))]
[Union(1, typeof(RailroadSquare))]
[Union(2, typeof(UtilitySquare))]
public interface IAsset
{
    int Appraise(Board board, Game game);
}
