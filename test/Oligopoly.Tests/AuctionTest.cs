using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Assets;
using Oligopoly.EventArgs;

namespace Oligopoly.Tests;

[TestClass]
public class AuctionTest
{
#nullable disable
    private Controller _controller;
    private Player _first;
    private Player _second;
    private Player _third;
#nullable enable

    [TestInitialize]
    public void Initialize()
    {
        _controller = Factory.CreateController(4, 5, 6, 4, 6, 4, 6, 4);
        _first = _controller.AddPlayer("Mark");
        _second = _controller.AddPlayer("John");
        _third = _controller.AddPlayer("Allison");
    }

    [TestMethod("Auction (1)")]
    public void TestAuctionTwoBidders()
    {
        TestAgent firstAgent = TestAgent
            .Create()
            .ThenBid(150, 150);
        TestAgent secondAgent = TestAgent
            .Create()
            .ThenBid(175, 175);
        Deed deed = _controller.Game.Deeds[19];

        _first.Agent = firstAgent;
        _first.SquareId = 11;
        _second.Agent = secondAgent;
        _controller.AuctionFailed += OnControllerAuctionEnded;
        _controller.AuctionSucceeded += OnControllerAuctionEnded;

        _controller.Start();
        _controller.MoveNext();
        Assert.AreEqual(2, deed.PlayerId);
        Assert.AreEqual(1500, _first.Cash);
        Assert.AreEqual(1325, _second.Cash);
    }

    private void OnControllerAuctionEnded(object? sender, AuctionEventArgs e)
    {
        Assert.IsTrue(e.Succeeded);
        Assert.IsNotNull(sender);

        Controller controller = (Controller)sender;

        Assert.AreEqual(controller.Game.Deeds[19], e.Asset);
        Assert.AreEqual(2, e.Bid.Bidder.Id);
        Assert.AreEqual(175, e.Bid.Amount);
    }

    [TestMethod("Auction (2)")]
    public void TestAuctionOneBidder()
    {
        Player third = _controller.AddPlayer("Allison");
        TestAgent firstAgent = TestAgent
            .Create()
            .ThenBid(150, 150);
        Deed deed = _controller.Game.Deeds[19];

        _first.Agent = firstAgent;
        _first.SquareId = 11;

        _controller.Start();
        _controller.MoveNext();
        Assert.AreEqual(1, deed.PlayerId);
        Assert.AreEqual(1350, _first.Cash);
        Assert.AreEqual(1500, _second.Cash);
        Assert.AreEqual(1500, third.Cash);
    }

    [TestMethod("Auction (3)")]
    public void TestAuctionFailedBidder()
    {
        TestAgent firstAgent = TestAgent
            .Create()
            .ThenBid(200, 200);
        TestAgent secondAgent = TestAgent
            .Create()
            .ThenBid(400, 400);
        TestAgent thirdAgent = TestAgent
            .Create()
            .ThenBid(300, 300);
        Deed deed = _controller.Game.Deeds[19];

        _first.Agent = firstAgent;
        _first.SquareId = 11;
        _second.Agent = secondAgent;
        _second.Cash = 250;
        _third.Agent = thirdAgent;

        _controller.Start();
        _controller.MoveNext();
        Assert.AreEqual(3, deed.PlayerId);
        Assert.AreEqual(1500, _first.Cash);
        Assert.AreEqual(250, _second.Cash);
        Assert.AreEqual(1200, _third.Cash);
    }
}
