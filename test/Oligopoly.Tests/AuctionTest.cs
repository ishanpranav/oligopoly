using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Dice;
using Oligopoly.Squares;

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
        TestAgent firstAgent = new TestAgent(nextBid: 150);
        TestAgent secondAgent = new TestAgent(nextBid: 175);
        ISquare square = board.Squares[19];
        Deed deed = game.Deeds[19];

        first.Agent = firstAgent;
        first.SquareId = 11;
        second.Agent = secondAgent;

        controller.Start();
        controller.Move(first);
        Assert.AreEqual(2, deed.PlayerId);
        Assert.AreEqual(1500, first.Cash);
        Assert.AreEqual(square, firstAgent.Auction!.Asset);
        Assert.AreEqual(second, firstAgent.Auction!.Bid.Bidder);
        Assert.AreEqual(175, firstAgent.Auction!.Bid.Amount);
        Assert.AreEqual(1325, second.Cash);
        Assert.AreEqual(square, secondAgent.Auction!.Asset);
        Assert.AreEqual(second, secondAgent.Auction!.Bid.Bidder);
        Assert.AreEqual(175, secondAgent.Auction!.Bid.Amount);
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
        TestAgent firstAgent = new TestAgent(nextBid: 150);
        TestAgent secondAgent = new TestAgent(nextBid: 0);
        TestAgent thirdAgent = new TestAgent(nextBid: 0);
        Deed deed = game.Deeds[19];

        first.Agent = firstAgent;
        first.SquareId = 11;
        second.Agent = secondAgent;
        third.Agent = thirdAgent;

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
        TestAgent firstAgent = new TestAgent(nextBid: 200);
        TestAgent secondAgent = new TestAgent(nextBid: 400);
        TestAgent thirdAgent = new TestAgent(nextBid: 300);
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
