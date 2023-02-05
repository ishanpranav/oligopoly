using MessagePack;
using Oligopoly.Auctions;

namespace Oligopoly.EventArgs;

[MessagePackObject]
public class AuctionEventArgs
{
    public AuctionEventArgs(IAsset asset)
    {
        Asset = asset;
    }

    public AuctionEventArgs(IAsset asset, Bid bid)
    {
        Asset = asset;
        Bid = bid;
    }

    [Key(0)]
    public IAsset Asset { get; }

    [Key(1)]
    public Bid Bid { get; }
}
