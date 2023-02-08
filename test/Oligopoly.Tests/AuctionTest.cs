using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Dice;
using Oligopoly.Squares;

namespace Oligopoly.Tests;

[TestClass]
public class AuctionTest
{
    [TestMethod]
    public void TestAuction()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(4, 5)));
        GameController controller = new GameController(board, game);
        Player first = controller.AddPlayer("Mark");
        Player second = controller.AddPlayer("John");
        TestAgent firstAgent = new TestAgent(nextBid: 150);
        TestAgent secondAgent = new TestAgent(nextBid: 175);

        first.Agent = firstAgent;
        second.Agent = secondAgent;
        first.SquareId = 11;

        controller.Start();
        controller.Move(first);

        ISquare square = board.Squares[19];
        Deed deed = game.Deeds[19];

        Assert.AreEqual(second.Id, deed.PlayerId);
        Assert.AreEqual(1500, first.Cash);
        Assert.AreEqual(square, firstAgent.Auction!.Asset);
        Assert.AreEqual(second, firstAgent.Auction!.Bid.Bidder);
        Assert.AreEqual(175, firstAgent.Auction!.Bid.Amount);
        Assert.AreEqual(1325, second.Cash);
        Assert.AreEqual(square, secondAgent.Auction!.Asset);
        Assert.AreEqual(second, secondAgent.Auction!.Bid.Bidder);
        Assert.AreEqual(175, secondAgent.Auction!.Bid.Amount);
    }
}
