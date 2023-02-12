using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Assets;
using Oligopoly.Dice;

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

        controller.Start();
        controller.Move(first);
        Assert.AreEqual(2, deed.PlayerId);
        Assert.AreEqual(1500, first.Cash);
        Assert.IsNotNull(firstAgent.Auction);
        Assert.IsNotNull(secondAgent.Auction);
        Assert.AreEqual(deed, firstAgent.Auction.Asset);
        Assert.IsTrue(firstAgent.Auction.Succeeded);
        Assert.AreEqual(second, firstAgent.Auction.Bid.Bidder);
        Assert.AreEqual(175, firstAgent.Auction!.Bid.Amount);
        Assert.AreEqual(1325, second.Cash);
        Assert.AreEqual(deed, secondAgent.Auction.Asset);
        Assert.IsTrue(secondAgent.Auction.Succeeded);
        Assert.AreEqual(second, secondAgent.Auction.Bid.Bidder);
        Assert.AreEqual(175, secondAgent.Auction.Bid.Amount);
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
