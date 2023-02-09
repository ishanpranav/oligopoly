using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Dice;

namespace Oligopoly.Tests;

[TestClass]
public class GameControllerTest
{
    [TestMethod("Salary (1)")]
    public void TestSalary()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(5, 6)));
        GameController controller = new GameController(board, game);
        Player player = controller.AddPlayer("Mark");

        player.SquareId = 40;

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(11, player.SquareId);
        Assert.AreEqual(1700, player.Cash);
    }

    [TestMethod("Salary (2)")]
    public void TestSalaryAdvance()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(1, 2)));
        GameController controller = new GameController(board, game);
        Player player = controller.AddPlayer("Mark");

        player.SquareId = 38;

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(1, player.SquareId);
        Assert.AreEqual(1700, player.Cash);
    }

    [TestMethod("Bankrupt (1)")]
    public void TestBankrupt()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(4, 6, 3, 1, 6, 4, 4, 6, 4, 6, 6, 4)));
        GameController controller = new GameController(board, game);
        Player first = controller.AddPlayer("Mark");
        Player second = controller.AddPlayer("John");
        Player third = controller.AddPlayer("Allison");
        Deed firstBlue = game.Deeds[37];
        Deed secondBlue = game.Deeds[39];
        Deed firstOrange = game.Deeds[16];
        Deed secondOrange = game.Deeds[18];
        Deed thirdOrange = game.Deeds[19];
        Deed firstRed = game.Deeds[21];

        first.SquareId = 1;
        second.Cash = 500;
        second.SquareId = 36;
        third.SquareId = 1;
        firstBlue.PlayerId = 1;
        firstBlue.Improvements = 4;
        secondBlue.PlayerId = 1;
        secondBlue.Improvements = 4;
        firstOrange.PlayerId = 2;
        firstOrange.Improvements = 2;
        secondOrange.PlayerId = 2;
        secondOrange.Improvements = 2;
        thirdOrange.PlayerId = 2;
        thirdOrange.Improvements = 2;
        firstRed.PlayerId = 2;
        firstRed.Mortgaged = true;

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(1978, first.Cash);
        Assert.IsFalse(game.Players.Contains(second));
        Assert.AreEqual(0, firstOrange.Improvements);
        Assert.AreEqual(1, firstOrange.PlayerId);
        Assert.AreEqual(0, secondOrange.Improvements);
        Assert.AreEqual(1, secondOrange.PlayerId);
        Assert.AreEqual(0, thirdOrange.Improvements);
        Assert.AreEqual(1, thirdOrange.PlayerId);
        Assert.IsTrue(firstRed.Mortgaged);
        Assert.AreEqual(1, firstRed.PlayerId);
        controller.MoveNext();
        Assert.AreEqual(21, first.SquareId);
        Assert.AreEqual(40, second.SquareId);
        Assert.AreEqual(21, third.SquareId);
    }

    [TestMethod("Bankrupt (2)")]
    public void TestBankruptJailbreak()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(6, 4, 1, 3, 6, 4, 2, 3)), shuffle: new TestShuffle(1));
        GameController controller = new GameController(board, game);
        Player first = controller.AddPlayer("Mark");
        Player second = controller.AddPlayer("John");

        second.Cash = 50;
        second.SquareId = 30;

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(1, second.CardIds.Count);
        Assert.AreEqual(15, game.DeckIds[1].Count);
        controller.MoveNext();
        Assert.AreEqual(16, game.DeckIds[1].Count);
        Assert.IsFalse(game.Players.Contains(second));
    }

    [TestMethod("Bankrupt (3)")]
    public void TestBankruptUngift()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(1, 1, 1, 2, 4, 6)), new TestShuffle(9));
        GameController controller = new GameController(board, game);
        Player first = controller.AddPlayer("Mark");
        Player second = controller.AddPlayer("John");
        Player third = controller.AddPlayer("Allison");

        second.Cash = 5;

        controller.Start();
        controller.MoveNext();
        Assert.IsTrue(game.Players.Contains(first));
        Assert.IsFalse(game.Players.Contains(second));
        Assert.IsTrue(game.Players.Contains(third));
        Assert.AreEqual(11, third.SquareId);
    }
}
