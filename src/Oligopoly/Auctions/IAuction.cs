using Oligopoly.Assets;

namespace Oligopoly.Auctions;

public interface IAuction
{
    Bid? Perform(Controller controller, Player player, IAsset asset);
}
