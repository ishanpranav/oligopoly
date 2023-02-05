using System;
using MessagePack;
using Oligopoly.Squares;

namespace Oligopoly.EventArgs;

[MessagePackObject]
public class AuctionEventArgs
{
    public AuctionEventArgs(IAsset asset)
    {
        Asset = asset;
    }

    public AuctionEventArgs(IAsset asset, Player player, int amount)
    {
        Asset = asset;
        Player = player;
        Amount = amount;
    }

    [Key(0)]
    public IAsset Asset { get; }

    [Key(1)]
    public Player? Player { get; }

    [Key(2)]
    public int Amount { get; }
}
