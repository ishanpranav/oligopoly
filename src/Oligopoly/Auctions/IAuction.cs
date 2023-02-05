namespace Oligopoly.Auctions;

public interface IAuction
{
    Bid Perform(GameController controller, IAsset asset);
}
