namespace Oligopoly.Auctions;

public interface IAuction
{
    void Auction(GameController controller, IAsset asset);
}
