using System;
using System.Collections.Generic;
using Oligopoly.EventArgs;

namespace Oligopoly.Auctions;

public class EnglishAuction : IAuction
{
    public event EventHandler<AuctionEventArgs>? AuctionSucceeded;
    public event EventHandler<AuctionEventArgs>? AuctionFailed;

    public void Auction(GameController controller, IAsset asset)
    {
        Console.WriteLine("{0} offered for auction", asset);

        int bid = 0;
        Queue<Player> bidders = new Queue<Player>(controller.Game.Players);

        while (bidders.Count > 1)
        {
            Player bidder = bidders.Dequeue();

            bid = bidder.Agent.Bid(controller.Game, bidder, asset);

            Console.WriteLine("{0} bids {1}", bidder, bid);

            if (bid <= 0)
            {
                continue;
            }

            controller.Tax(bidder, bid);

            if (bidder.Cash < 0)
            {
                controller.Untax(bidder, bid);

                continue;
            }

            bidders.Enqueue(bidder);
        }

        if (bidders.Count is 1)
        {
            OnAuctionSucceeded(new AuctionEventArgs(asset, bidders.Dequeue(), bid));
        }
        else
        {
            OnAuctionFailed(new AuctionEventArgs(asset));
        }
    }

    protected virtual void OnAuctionSucceeded(AuctionEventArgs e)
    {
        if (AuctionSucceeded is not null)
        {
            AuctionSucceeded(sender: this, e);
        }
    }

    protected virtual void OnAuctionFailed(AuctionEventArgs e)
    {
        if (AuctionFailed is not null)
        {
            AuctionFailed(sender: this, e);
        }
    }
}
