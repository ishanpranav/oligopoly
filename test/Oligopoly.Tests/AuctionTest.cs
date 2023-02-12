using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Assets;
using Oligopoly.Dice;
using Oligopoly.EventArgs;

namespace Oligopoly.Tests;

[TestClass]
public class AuctionTest
{
    [TestMethod("Auction (1)")]
    public void TestAuctionTwoBidders()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(4, 5)));
        GameController controller = new GameController(board, game);
        Player first = controller.AddPlayer("Mark");
        Player second = controller.AddPlayer("John");
        TestAgent firstAgent = TestAgent
            .Create()
            .ThenBid(150, 150);
        TestAgent secondAgent = TestAgent
            .Create()
            .ThenBid(175, 175);
        Deed deed = game.Deeds[19];

        first.Agent = firstAgent;
        first.SquareId = 11;
        second.Agent = secondAgent;
        controller.AuctionFailed += OnControllerAuctionEnded;
        controller.AuctionSucceeded += OnControllerAuctionEnded;

        controller.Start();
        controller.Move(first);
        Assert.AreEqual(2, deed.PlayerId);
        Assert.AreEqual(1500, first.Cash);
        Assert.AreEqual(1325, second.Cash);
    }

    private void OnControllerAuctionEnded(object? sender, AuctionEventArgs e)
    {
        Assert.IsTrue(e.Succeeded);
        Assert.IsNotNull(sender);

        GameController controller = (GameController)sender;

        Assert.AreEqual(controller.Game.Deeds[19], e.Asset);
        Assert.AreEqual(2, e.Bid.Bidder.Id);
        Assert.AreEqual(175, e.Bid.Amount);
    }

    [TestMethod("Auction (2)")]
    public void TestAuctionOneBidder()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(4, 5)));
        GameController controller = new GameController(board, game);
        Player first = controller.AddPlayer("Mark");
        Player second = controller.AddPlayer("John");
        Player third = controller.AddPlayer("Allison");
        TestAgent firstAgent = TestAgent
            .Create()
            .ThenBid(150, 150);
        Deed deed = game.Deeds[19];

        first.Agent = firstAgent;
        first.SquareId = 11;

        controller.Start();
        controller.Move(first);
        Assert.AreEqual(1, deed.PlayerId);
        Assert.AreEqual(1350, first.Cash);
        Assert.AreEqual(1500, second.Cash);
        Assert.AreEqual(1500, third.Cash);
    }

    [TestMethod("Auction (3)")]
    public void TestAuctionFailedBidder()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(4, 5)));
        GameController controller = new GameController(board, game);
        Player first = controller.AddPlayer("Mark");
        Player second = controller.AddPlayer("John");
        Player third = controller.AddPlayer("Allison");
        TestAgent firstAgent = TestAgent
            .Create()
            .ThenBid(200, 200);
        TestAgent secondAgent = TestAgent
            .Create()
            .ThenBid(400, 400);
        TestAgent thirdAgent = TestAgent
            .Create()
            .ThenBid(300, 300);
        Deed deed = game.Deeds[19];

        first.Agent = firstAgent;
        first.SquareId = 11;
        second.Agent = secondAgent;
        second.Cash = 250;
        third.Agent = thirdAgent;

        controller.Start();
        controller.Move(first);
        Assert.AreEqual(3, deed.PlayerId);
        Assert.AreEqual(1500, first.Cash);
        Assert.AreEqual(250, second.Cash);
        Assert.AreEqual(1200, third.Cash);
    }
}
