using System.Diagnostics.CodeAnalysis;
using MessagePack;
using Oligopoly.Assets;

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
    public Bid? Bid { get; }

    [IgnoreMember]
    [MemberNotNullWhen(true, nameof(Bid))]
    public bool Succeeded
    {
        get
        {
            return Bid is not null;
        }
    }
}
