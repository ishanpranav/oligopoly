using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Dice;

namespace Oligopoly.Tests;

[TestClass]
public class GiftCardTest
{
    [TestMethod("Draw")]
    public void TestDraw()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(4, 6, 3, 4, 4, 6, 4, 6)), new TestShuffle(15));
        GameController controller = new GameController(board, game);
        Player first = controller.AddPlayer("Mark");
        Player second = controller.AddPlayer("John");
        Player third = controller.AddPlayer("Allison");
        Player fourth = controller.AddPlayer("Elizabeth");

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(1550, first.Cash);
        Assert.AreEqual(1350, second.Cash);
        Assert.AreEqual(1550, third.Cash);
        Assert.AreEqual(1550, fourth.Cash);
    }
}
