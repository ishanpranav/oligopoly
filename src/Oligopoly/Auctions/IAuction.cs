using Oligopoly.Assets;

namespace Oligopoly.Auctions;

public interface IAuction
{
    Bid? Perform(GameController controller, Player player, IAsset asset);
}
