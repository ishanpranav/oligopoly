using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Nito.Collections;
using Oligopoly.Agents;
using Oligopoly.EventArgs;

namespace Oligopoly.Tests;

internal sealed class TestAgent : Agent
{
    private readonly Queue<int> _mortgages = new Queue<int>();
    private readonly Queue<int> _improvements = new Queue<int>();
    private readonly Queue<int> _bids = new Queue<int>();
    private readonly Queue<Warning> _warnings = new Queue<Warning>();

    private TestAgent() { }

    public AuctionEventArgs? Auction { get; private set; }

    public TestAgent ThenMortgage(int value)
    {
        _mortgages.Enqueue(value);

        return this;
    }

    public TestAgent ThenImprove(params int[] values)
    {
        foreach (int value in values)
        {
            _improvements.Enqueue(value);
        }

        return this;
    }

    public TestAgent ThenBid(params int[] values)
    {
        foreach (int value in values)
        {
            _bids.Enqueue(value);
        }

        return this;
    }

    public TestAgent Expect(Warning value)
    {
        _warnings.Enqueue(value);

        return this;
    }

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
    public override int Mortgage(Game game, Player player)
    {
        _mortgages.TryDequeue(out int id);

        return id;
    }

    /// <inheritdoc/>
    public override int Improve(Game game, Player player)
    {
        _improvements.TryDequeue(out int id);

        return id;
    }

    /// <inheritdoc/>
    public override int Bid(Game game, Player player, IAsset asset, int bid)
    {
        _bids.TryDequeue(out int id);

        return id;
    }

    /// <inheritdoc/>
    public override void Warn(Game game, Player player, Warning warning)
    {
        _warnings.TryDequeue(out Warning expectedWarning);

        Assert.AreEqual(expectedWarning, warning);
    }

    public static TestAgent Create()
    {
        return new TestAgent();
    }
}
