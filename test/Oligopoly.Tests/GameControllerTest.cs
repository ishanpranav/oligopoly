using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Assets;
using Oligopoly.EventArgs;

namespace Oligopoly.Tests;

[TestClass]
public class GameControllerTest
{
    [TestMethod("Salary (1)")]
    public void TestSalary()
    {
        GameController controller = Factory.CreateController(5, 6, 6, 4);
        Player player = controller.AddPlayer("Mark");

        player.SquareId = 40;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(11, player.SquareId);
        Assert.AreEqual(1700, player.Cash);
    }

    [TestMethod("Salary (2)")]
    public void TestSalaryAdvance()
    {
        GameController controller = Factory.CreateController(1, 2, 4, 6);
        Player player = controller.AddPlayer("Mark");

        player.SquareId = 38;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(1, player.SquareId);
        Assert.AreEqual(1700, player.Cash);
    }

    [TestMethod("Transfer")]
    public void TestTransfer()
    {
        GameController controller = Factory.CreateController(4, 6, 3, 1, 6, 4, 4, 6, 4, 6, 6, 4);
        Game game = controller.Game;
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

    [TestMethod("Improve (1)")]
    public void TestImprove()
    {
        GameController controller = Factory.CreateController(4, 1, 6, 4);
        Game game = controller.Game;
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

        controller.AddPlayer("John");
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
        GameController controller = Factory.CreateController(4, 1, 6, 4);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed firstRed = game.Deeds[21];
        Deed secondRed = game.Deeds[23];

        player.Agent = TestAgent
            .Create()
            .ThenImprove(22, 24)
            .ThenExpect(Warning.GroupAccessDenied);
        firstRed.PlayerId = 1;
        secondRed.PlayerId = 1;

        controller.AddPlayer("John");
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
        GameController controller = Factory.CreateController(4, 1, 6, 4);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed firstRed = game.Deeds[21];
        Deed secondRed = game.Deeds[23];
        Deed thirdRed = game.Deeds[24];

        player.Agent = TestAgent
            .Create()
            .ThenExpect(Warning.InsufficientCards);
        player.Cash = 800;
        player.SquareId = 16;
        firstRed.PlayerId = 1;
        secondRed.PlayerId = 1;
        thirdRed.PlayerId = 1;

        controller.AddPlayer("John");
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
        GameController controller = Factory.CreateController(4, 1, 6, 4);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed firstRed = game.Deeds[21];
        Deed secondRed = game.Deeds[23];
        Deed thirdRed = game.Deeds[24];

        player.Agent = TestAgent
            .Create()
            .ThenImprove(22, 24, 25, 22, 24, 25)
            .ThenMortgage(25)
            .ThenExpect(Warning.GroupMortgaged);
        player.Cash = 800;
        player.SquareId = 16;
        firstRed.PlayerId = 1;
        secondRed.PlayerId = 1;
        thirdRed.PlayerId = 1;

        controller.AddPlayer("John");
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
        GameController controller = Factory.CreateController(4, 1, 6, 4);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed firstRed = game.Deeds[21];
        Deed secondRed = game.Deeds[23];
        Deed thirdRed = game.Deeds[24];

        player.Agent = TestAgent
            .Create()
            .ThenImprove(22, 24, 25, 22, 24, 25, 22, 24, 25, 22, 24, 25, 22, 24, 25, 22, 24, 25)
            .ThenExpect(Warning.Improved);
        player.Cash = 3000;
        player.SquareId = 16;
        firstRed.PlayerId = 1;
        secondRed.PlayerId = 1;
        thirdRed.PlayerId = 1;

        controller.AddPlayer("Mark");
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
        GameController controller = Factory.CreateController(4, 1, 6, 4);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed firstRed = game.Deeds[21];
        Deed secondRed = game.Deeds[23];
        Deed thirdRed = game.Deeds[24];
        Deed firstBrown = game.Deeds[1];
        Deed secondBrown = game.Deeds[3];

        player.Agent = TestAgent
            .Create()
            .ThenImprove(22, 24, 25, 22, 24, 25, 22, 24, 25, 25, 2, 4, 25)
            .ThenExpect(Warning.UnbalancedImprovements);
        player.Cash = 2000;
        player.SquareId = 16;
        firstRed.PlayerId = 1;
        secondRed.PlayerId = 1;
        thirdRed.PlayerId = 1;
        firstBrown.PlayerId = 1;
        secondBrown.PlayerId = 1;

        controller.AddPlayer("John");
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
        GameController controller = Factory.CreateController(4, 1, 4, 1);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed firstBrown = game.Deeds[1];
        Deed secondBrown = game.Deeds[3];

        player.Agent = TestAgent
            .Create()
            .ThenImprove(2, 4)
            .ThenExpect(Warning.AccessDenied);
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
        GameController controller = Factory.CreateController(3, 3, 1, 1, 6, 4, 6, 4, 1, 2);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed firstOrange = game.Deeds[16];
        Deed secondOrange = game.Deeds[18];
        Deed thirdOrange = game.Deeds[19];

        player.Agent = TestAgent
            .Create()
            .ThenOffer(true, true);
        player.SquareId = 11;

        controller.AddPlayer("John");
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
        GameController controller = Factory.CreateController(3, 3, 1, 1, 6, 4, 6, 4, 1, 2);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed firstOrange = game.Deeds[16];
        Deed secondOrange = game.Deeds[18];
        Deed thirdOrange = game.Deeds[19];

        player.Agent = TestAgent
            .Create()
            .ThenOffer(true, true)
            .ThenExpect(Warning.InsufficientCash);
        player.Cash = 320;
        player.SquareId = 11;

        controller.AddPlayer("John");
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
        GameController controller = Factory.CreateController(4, 6, 4, 6);
        Player first = controller.AddPlayer("Mark");
        Player second = controller.AddPlayer("John");
        Deed deed = controller.Game.Deeds[39];

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
        GameController controller = Factory.CreateController(4, 6, 4, 6);
        Player first = controller.AddPlayer("Mark");
        Player second = controller.AddPlayer("John");
        Deed deed = controller.Game.Deeds[39];

        first.Agent = TestAgent
            .Create()
            .ThenPropose(new Offer(second, deed, amount: 1000))
            .ThenExpect(Warning.AccessDenied);
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
        GameController controller = Factory.CreateController(4, 6, 4, 6);
        Game game = controller.Game;
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
        GameController controller = Factory.CreateController(4, 6, 4, 6);
        Game game = controller.Game;
        Player first = controller.AddPlayer("Mark");
        Player second = controller.AddPlayer("John");
        Deed deed = game.Deeds[39];

        first.Agent = TestAgent
            .Create()
            .ThenPropose(new Offer(second, deed, amount: 1000))
            .ThenExpect(Warning.Improved);
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

    [TestMethod("Jailbreak (1)")]
    public void TestJailbreak()
    {
        GameController controller = Factory.CreateController(2, 3, 6, 4, 1, 4, 6, 4, 6, 4, 6, 4);
        Player player = controller.AddPlayer("Mark");

        player.SquareId = 11;
        player.Sentence = 3;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(11, player.SquareId);
        Assert.AreEqual(2, player.Sentence);
        controller.MoveNext();
        Assert.AreEqual(11, player.SquareId);
        Assert.AreEqual(1, player.Sentence);
        controller.MoveNext();
        Assert.AreEqual(21, player.SquareId);
        Assert.AreEqual(0, player.Sentence);
        Assert.AreEqual(1450, player.Cash);
    }

    [TestMethod("Jailbreak (2)")]
    public void TestJailbreakNone()
    {
        GameController controller = Factory.CreateController(2, 3, 6, 4, 1, 4, 6, 4, 5, 5, 6, 4);
        Player player = controller.AddPlayer("Mark");

        player.SquareId = 11;
        player.Sentence = 3;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(11, player.SquareId);
        Assert.AreEqual(2, player.Sentence);
        controller.MoveNext();
        Assert.AreEqual(11, player.SquareId);
        Assert.AreEqual(1, player.Sentence);
        controller.MoveNext();
        Assert.AreEqual(21, player.SquareId);
        Assert.AreEqual(0, player.Sentence);
        Assert.AreEqual(1500, player.Cash);
    }

    [TestMethod("Jailbreak (3)")]
    public void TestJailbreakBail()
    {
        GameController controller = Factory.CreateController(3, 5, 6, 4);
        Player player = controller.AddPlayer("Mark");

        player.Agent = TestAgent
            .Create()
            .ThenJailbreak(JailbreakStrategy.Bail);
        player.SquareId = 11;
        player.Sentence = 3;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(19, player.SquareId);
        Assert.AreEqual(0, player.Sentence);
        Assert.AreEqual(1450, player.Cash);
    }

    [TestMethod("Jailbreak (4)")]
    public void TestJailbreakCard()
    {
        GameController controller = Factory.CreateController(3, 5, 6, 4);
        Player player = controller.AddPlayer("Mark");

        player.Agent = TestAgent
            .Create()
            .ThenJailbreak(JailbreakStrategy.Card);
        player.SquareId = 11;
        player.Sentence = 3;

        player.CardIds.Enqueue(new CardId(id: 1, deckId: 1));
        player.CardIds.Enqueue(new CardId(id: 1, deckId: 2));
        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(19, player.SquareId);
        Assert.AreEqual(0, player.Sentence);
        Assert.AreEqual(1500, player.Cash);
        Assert.AreEqual(1, player.CardIds.Count);
    }

    [TestMethod("Jailbreak (5)")]
    public void TestJailbreakInsufficientCards()
    {
        GameController controller = Factory.CreateController(3, 5, 6, 4);
        Player player = controller.AddPlayer("Mark");

        player.Agent = TestAgent
            .Create()
            .ThenJailbreak(JailbreakStrategy.Card)
            .ThenExpect(Warning.InsufficientCards);
        player.SquareId = 11;
        player.Sentence = 3;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(11, player.SquareId);
        Assert.AreEqual(2, player.Sentence);
        Assert.AreEqual(1500, player.Cash);
        Assert.AreEqual(0, player.CardIds.Count);
    }

    [TestMethod("Jailbreak (6)")]
    public void TestJailbreakPolice()
    {
        GameController controller = Factory.CreateController(5, 5, 6, 4, 6, 4, 6, 4);
        Player player = controller.AddPlayer("Mark");

        player.Agent = TestAgent
            .Create()
            .ThenJailbreak(JailbreakStrategy.Bail);
        player.SquareId = 11;
        player.Sentence = 3;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(11, player.SquareId);
        Assert.AreEqual(3, player.Sentence);
        Assert.AreEqual(1450, player.Cash);
    }

    [TestMethod("Police")]
    public void TestPolice()
    {
        GameController controller = Factory.CreateController(2, 2, 3, 3, 5, 5, 6, 4);
        Player player = controller.AddPlayer("Mark");

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(11, player.SquareId);
        Assert.AreEqual(3, player.Sentence);
    }

    [TestMethod("Ended")]
    public void TestEnded()
    {
        GameController controller = Factory.CreateController(4, 6, 4, 6, 4, 6, 4, 6, 4, 6, 4, 6, 4, 6, 4, 6, 4, 6, 4, 6, 4, 6);
        Player first = controller.AddPlayer("Mark");
        Player second = controller.AddPlayer("John");
        TestAgent firstAgent = TestAgent.Create();
        TestAgent secondAgent = TestAgent.Create();

        first.Agent = firstAgent;
        second.Agent = secondAgent;
        first.Cash = 0;
        controller.Ended += OnControllerEnded;

        controller.Start();

        while (controller.MoveNext())
        {
            Assert.IsTrue(controller.Game.Players.Count > 1);
        }
    }

    private void OnControllerEnded(object? sender, PlayerEventArgs e)
    {
        Assert.AreEqual(2, e.Player.Id);
    }
}
