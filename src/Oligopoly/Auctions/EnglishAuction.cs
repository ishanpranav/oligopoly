using System;
using System.Collections.Generic;

namespace Oligopoly.Auctions;

public class EnglishAuction : IAuction
{
    /// <inheritdoc/>
    public Bid Perform(GameController controller, IAsset asset)
    {
        Console.WriteLine("{0} offered for auction", asset);

        int bid = 0;
        Queue<Player> bidders = new Queue<Player>(controller.Game.Players);

        while (bidders.Count > 0)
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
            
            if (bidders.Count is 0)
            {
                return new Bid(bidders.Dequeue(), bid);
            }

            bidders.Enqueue(bidder);
        }

        return Bid.Empty;
    }
}
