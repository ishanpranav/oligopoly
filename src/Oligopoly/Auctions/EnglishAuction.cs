using System;
using System.Collections.Generic;
using Oligopoly.Assets;

namespace Oligopoly.Auctions;

public class EnglishAuction : IAuction
{
    /// <inheritdoc/>
    public Bid? Perform(GameController controller, Player player, IAsset asset)
    {
        int previousBid = 1;
        Queue<Player> bidders = new Queue<Player>(controller.Game.Players);

        while (bidders.Count > 0)
        {
            Player bidder = bidders.Dequeue();
            int bid = bidder.Agent.Bid(controller.Game, player, new Offer(bidder, asset, previousBid));

            if (bid < previousBid || bid > bidder.Cash)
            {
                continue;
            }

            previousBid = bid;

            bidders.Enqueue(bidder);

            if (bidders.Count is 1)
            {
                return new Bid(bidders.Dequeue(), bid);
            }
        }

        return null;
    }
}
