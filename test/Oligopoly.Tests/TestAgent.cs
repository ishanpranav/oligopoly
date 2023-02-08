using Oligopoly.Agents;
using Oligopoly.EventArgs;

namespace Oligopoly.Tests;

internal sealed class TestAgent : Agent
{
    private readonly int _nextBid;

    public TestAgent(int nextBid)
    {
        _nextBid = nextBid;
    }

    public AuctionEventArgs? Auction { get; private set; }

    /// <inheritdoc/>
    public override void Connect(GameController controller)
    {
        controller.AuctionSucceeded += AuctionEnded;
        controller.AuctionFailed += AuctionEnded;
    }

    private void AuctionEnded(object? sender, AuctionEventArgs e)
    {
        Auction = e;
    }

    /// <inheritdoc/>
    public override int Bid(Game game, Player player, IAsset asset, int bid)
    {
        return _nextBid;
    }
}
