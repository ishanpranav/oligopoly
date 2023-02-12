using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Assets;
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

    [TestMethod("Transfer (1)")]
    public void TestTransfer()
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
        second.Agent = TestAgent
            .Create()
            .ThenUnimprove(20, 19, 17, 20, 19, 17);
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
        game.Houses -= 14;

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(2278, first.Cash);
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
        Assert.AreEqual(24, game.Houses);
        Assert.AreEqual(12, game.Hotels);
    }

    [TestMethod("Transfer (2)")]
    public void TestTransferUngift()
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

    [TestMethod("Discard")]
    public void TestDiscard()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(1, 3, 2, 3)), shuffle: new TestShuffle(1));
        GameController controller = new GameController(board, game);
        Player first = controller.AddPlayer("Mark");

        first.Cash = 50;
        first.SquareId = 30;

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(1, first.CardIds.Count);
        Assert.AreEqual(15, game.DeckIds[1].Count);
        controller.MoveNext();
        Assert.AreEqual(16, game.DeckIds[1].Count);
        Assert.IsFalse(game.Players.Contains(first));
    }

    [TestMethod("Improve (1)")]
    public void TestImprove()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(4, 1)));
        GameController controller = new GameController(board, game);
        Player player = controller.AddPlayer("Mark");
        Deed firstRed = game.Deeds[21];
        Deed secondRed = game.Deeds[23];
        Deed thirdRed = game.Deeds[24];
        Deed firstBrown = game.Deeds[1];
        Deed secondBrown = game.Deeds[3];
        player.Agent = TestAgent
            .Create()
            .ThenImprove(22, 24, 25, 22, 24, 25, 25, 2, 4);
        player.SquareId = 16;
        firstRed.PlayerId = 1;
        secondRed.PlayerId = 1;
        thirdRed.PlayerId = 1;
        firstBrown.PlayerId = 1;
        secondBrown.PlayerId = 1;

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(2, firstRed.Improvements);
        Assert.AreEqual(2, secondRed.Improvements);
        Assert.AreEqual(3, thirdRed.Improvements);
        Assert.AreEqual(1, firstBrown.Improvements);
        Assert.AreEqual(1, secondBrown.Improvements);
        Assert.AreEqual(350, player.Cash);
        Assert.AreEqual(23, game.Houses);
        Assert.AreEqual(12, game.Hotels);
    }

    [TestMethod("Improve (2)")]
    public void TestImproveGroupAccessDenied()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(4, 1)));
        GameController controller = new GameController(board, game);
        Player player = controller.AddPlayer("Mark");
        Deed firstRed = game.Deeds[21];
        Deed secondRed = game.Deeds[23];

        player.Agent = TestAgent
            .Create()
            .ThenImprove(22, 24)
            .Expect(Warning.GroupAccessDenied);
        firstRed.PlayerId = 1;
        secondRed.PlayerId = 1;

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(0, firstRed.Improvements);
        Assert.AreEqual(0, secondRed.Improvements);
        Assert.AreEqual(1500, player.Cash);
        Assert.AreEqual(32, game.Houses);
        Assert.AreEqual(12, game.Hotels);
    }

    [TestMethod("Improve (3)")]
    public void TestImproveInsufficientCash()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(4, 1)));
        GameController controller = new GameController(board, game);
        Player player = controller.AddPlayer("Mark");
        Deed firstRed = game.Deeds[21];
        Deed secondRed = game.Deeds[23];
        Deed thirdRed = game.Deeds[24];

        player.Agent = TestAgent
            .Create()
            .Expect(Warning.InsufficientCards);
        player.Cash = 800;
        player.SquareId = 16;
        firstRed.PlayerId = 1;
        secondRed.PlayerId = 1;
        thirdRed.PlayerId = 1;

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(0, firstRed.Improvements);
        Assert.AreEqual(0, secondRed.Improvements);
        Assert.AreEqual(0, thirdRed.Improvements);
        Assert.AreEqual(800, player.Cash);
        Assert.AreEqual(32, game.Houses);
        Assert.AreEqual(12, game.Hotels);
    }

    [TestMethod("Improve (4)")]
    public void TestImproveMortgage()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(4, 1)));
        GameController controller = new GameController(board, game);
        Player player = controller.AddPlayer("Mark");
        Deed firstRed = game.Deeds[21];
        Deed secondRed = game.Deeds[23];
        Deed thirdRed = game.Deeds[24];

        player.Agent = TestAgent
            .Create()
            .ThenImprove(22, 24, 25, 22, 24, 25)
            .ThenMortgage(25)
            .Expect(Warning.GroupMortgaged);
        player.Cash = 800;
        player.SquareId = 16;
        firstRed.PlayerId = 1;
        secondRed.PlayerId = 1;
        thirdRed.PlayerId = 1;

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(0, firstRed.Improvements);
        Assert.AreEqual(0, secondRed.Improvements);
        Assert.AreEqual(0, thirdRed.Improvements);
        Assert.AreEqual(920, player.Cash);
        Assert.AreEqual(32, game.Houses);
        Assert.AreEqual(12, game.Hotels);
    }

    [TestMethod("Improve (5)")]
    public void TestImproveImproved()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(4, 1)));
        GameController controller = new GameController(board, game);
        Player player = controller.AddPlayer("Mark");
        Deed firstRed = game.Deeds[21];
        Deed secondRed = game.Deeds[23];
        Deed thirdRed = game.Deeds[24];

        player.Agent = TestAgent
            .Create()
            .ThenImprove(22, 24, 25, 22, 24, 25, 22, 24, 25, 22, 24, 25, 22, 24, 25, 22, 24, 25)
            .Expect(Warning.Improved);
        player.Cash = 3000;
        player.SquareId = 16;
        firstRed.PlayerId = 1;
        secondRed.PlayerId = 1;
        thirdRed.PlayerId = 1;

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(5, firstRed.Improvements);
        Assert.AreEqual(5, secondRed.Improvements);
        Assert.AreEqual(5, thirdRed.Improvements);
        Assert.AreEqual(750, player.Cash);
        Assert.AreEqual(32, game.Houses);
        Assert.AreEqual(9, game.Hotels);
    }

    [TestMethod("Improve (6)")]
    public void TestImproveUnbalancedImprovements()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(4, 1)));
        GameController controller = new GameController(board, game);
        Player player = controller.AddPlayer("Mark");
        Deed firstRed = game.Deeds[21];
        Deed secondRed = game.Deeds[23];
        Deed thirdRed = game.Deeds[24];
        Deed firstBrown = game.Deeds[1];
        Deed secondBrown = game.Deeds[3];

        player.Agent = TestAgent
            .Create()
            .ThenImprove(22, 24, 25, 22, 24, 25, 22, 24, 25, 25, 2, 4, 25)
            .Expect(Warning.UnbalancedImprovements);
        player.Cash = 2000;
        player.SquareId = 16;
        firstRed.PlayerId = 1;
        secondRed.PlayerId = 1;
        thirdRed.PlayerId = 1;
        firstBrown.PlayerId = 1;
        secondBrown.PlayerId = 1;

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(3, firstRed.Improvements);
        Assert.AreEqual(3, secondRed.Improvements);
        Assert.AreEqual(4, thirdRed.Improvements);
        Assert.AreEqual(1, firstBrown.Improvements);
        Assert.AreEqual(1, secondBrown.Improvements);
        Assert.AreEqual(400, player.Cash);
        Assert.AreEqual(20, game.Houses);
        Assert.AreEqual(12, game.Hotels);
    }

    [TestMethod("Improve (7)")]
    public void TestImproveAccessDenied()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(4, 1, 4, 1)));
        GameController controller = new GameController(board, game);
        Player player = controller.AddPlayer("Mark");
        Deed firstBrown = game.Deeds[1];
        Deed secondBrown = game.Deeds[3];

        player.Agent = TestAgent
            .Create()
            .ThenImprove(2, 4)
            .Expect(Warning.AccessDenied);
        player.SquareId = 16;
        firstBrown.PlayerId = 2;
        secondBrown.PlayerId = 2;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(0, firstBrown.Improvements);
        Assert.AreEqual(0, secondBrown.Improvements);
        Assert.AreEqual(1500, player.Cash);
        Assert.AreEqual(32, game.Houses);
        Assert.AreEqual(12, game.Hotels);
    }

    [TestMethod("Offer (1)")]
    public void TestOffer()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(3, 3, 1, 1, 1, 2)));
        GameController controller = new GameController(board, game);
        Player player = controller.AddPlayer("Mark");
        Deed firstOrange = game.Deeds[16];
        Deed secondOrange = game.Deeds[18];
        Deed thirdOrange = game.Deeds[19];

        player.Agent = TestAgent
            .Create()
            .ThenOffer(true, true);
        player.SquareId = 11;

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(1, firstOrange.PlayerId);
        Assert.AreEqual(1, secondOrange.PlayerId);
        Assert.AreEqual(0, thirdOrange.PlayerId);
        Assert.AreEqual(1140, player.Cash);
    }

    [TestMethod("Offer (2)")]
    public void TestOfferInsufficientCash()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(3, 3, 1, 1, 1, 2)));
        GameController controller = new GameController(board, game);
        Player player = controller.AddPlayer("Mark");
        Deed firstOrange = game.Deeds[16];
        Deed secondOrange = game.Deeds[18];
        Deed thirdOrange = game.Deeds[19];

        player.Agent = TestAgent
            .Create()
            .ThenOffer(true, true)
            .Expect(Warning.InsufficientCash);
        player.Cash = 320;
        player.SquareId = 11;

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(1, firstOrange.PlayerId);
        Assert.AreEqual(0, secondOrange.PlayerId);
        Assert.AreEqual(0, thirdOrange.PlayerId);
        Assert.AreEqual(140, player.Cash);
    }

    [TestMethod("Propose (1)")]
    public void TestPropose()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(4, 6, 4, 6)));
        GameController controller = new GameController(board, game);
        Player first = controller.AddPlayer("Mark");
        Player second = controller.AddPlayer("John");
        Deed deed = game.Deeds[39];

        first.Agent = TestAgent
            .Create()
            .ThenPropose(new Offer(second, deed, amount: 1000));
        second.Agent = TestAgent
            .Create()
            .ThenRespond(true);
        deed.PlayerId = 2;

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(1, deed.PlayerId);
        Assert.AreEqual(500, first.Cash);
        Assert.AreEqual(2500, second.Cash);
    }

    [TestMethod("Propose (2)")]
    public void TestProposeAccessDenied()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(4, 6, 4, 6)));
        GameController controller = new GameController(board, game);
        Player first = controller.AddPlayer("Mark");
        Player second = controller.AddPlayer("John");
        Deed deed = game.Deeds[39];

        first.Agent = TestAgent
            .Create()
            .ThenPropose(new Offer(second, deed, amount: 1000))
            .Expect(Warning.AccessDenied);
        second.Agent = TestAgent
            .Create()
            .ThenRespond(true);

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(0, deed.PlayerId);
        Assert.AreEqual(1500, first.Cash);
        Assert.AreEqual(1500, second.Cash);
    }

    [TestMethod("Propose (3)")]
    public void TestProposeDiscard()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(4, 6, 4, 6)));
        GameController controller = new GameController(board, game);
        Player first = controller.AddPlayer("Mark");
        Player second = controller.AddPlayer("John");
        Deed deed = game.Deeds[39];

        first.Agent = TestAgent
            .Create()
            .ThenPropose(new Offer(second, deed, amount: 1000));
        first.Cash = 700;
        second.Agent = TestAgent
            .Create()
            .ThenRespond(true);
        deed.PlayerId = 2;

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(0, deed.PlayerId);
        Assert.AreEqual(2200, second.Cash);
        Assert.IsFalse(game.Players.Contains(first));
    }

    [TestMethod("Propose (4)")]
    public void TestProposeImproved()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board, new D6PairDice(new TestRandom(4, 6, 4, 6)));
        GameController controller = new GameController(board, game);
        Player first = controller.AddPlayer("Mark");
        Player second = controller.AddPlayer("John");
        Deed deed = game.Deeds[39];

        first.Agent = TestAgent
            .Create()
            .ThenPropose(new Offer(second, deed, amount: 1000))
            .Expect(Warning.Improved);
        first.Cash = 700;
        second.Agent = TestAgent
            .Create()
            .ThenRespond(true);
        deed.PlayerId = 2;
        deed.Improvements = 3;
        game.Houses -= 3;

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(2, deed.PlayerId);
        Assert.AreEqual(700, first.Cash);
        Assert.AreEqual(1500, second.Cash);
        Assert.AreEqual(29, game.Houses);
        Assert.AreEqual(12, game.Hotels);
    }
}
