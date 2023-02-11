using Oligopoly.Assets;

namespace Oligopoly;

public class Offer
{
    public Offer(Player player, IAsset asset, int amount)
    {
        Player = player;
        Asset = asset;
        Amount = amount;
    }

    public Player Player { get; }
    public IAsset Asset { get; }
    public int Amount { get; }
}
