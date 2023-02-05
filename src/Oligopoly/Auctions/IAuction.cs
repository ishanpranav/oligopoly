using Oligopoly.Squares;

namespace Oligopoly.Auctions;

public interface IAuction
{
    void Auction(GameController controller, IAsset asset);
}
