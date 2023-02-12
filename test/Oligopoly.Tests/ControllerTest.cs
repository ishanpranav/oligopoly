using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Assets;
using Oligopoly.EventArgs;

namespace Oligopoly.Tests;

[TestClass]
public class ControllerTest
{
    [DataRow(38, 1, 2, 1, DisplayName = "Salary (1)")]
    [DataRow(40, 5, 6, 11, DisplayName = "Salary (2)")]
    [DataTestMethod]
    public void TestSalary(int squareId, int first, int second, int destinationSquareId)
    {
        Controller controller = Factory.CreateController(first, second, 6, 4);
        Player player = controller.AddPlayer("Mark");

        player.SquareId = squareId;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(destinationSquareId, player.SquareId);
        Assert.AreEqual(1700, player.Cash);
    }

    [TestMethod("Transfer")]
    public void TestTransfer()
    {
        Controller controller = Factory.CreateController(4, 6, 3, 1, 6, 4, 4, 6, 4, 6, 6, 4);
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
        Assert.AreEqual(2289, first.Cash);
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

    [TestMethod("Mortgage (1)")]
    public void TestMortgage()
    {
        Controller controller = Factory.CreateController(1, 2, 6, 4);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed railroad = game.Deeds[5];
        Deed firstBlue = game.Deeds[37];
        Deed firstOrange = game.Deeds[16];
        Deed secondOrange = game.Deeds[18];
        Deed thirdOrange = game.Deeds[19];

        player.Agent = TestAgent
            .Create()
            .ThenMortgage(17, 6);
        player.SquareId = 36;
        railroad.PlayerId = 1;
        firstBlue.PlayerId = 1;
        firstOrange.PlayerId = 1;
        secondOrange.PlayerId = 1;
        thirdOrange.PlayerId = 1;

        controller.AddPlayer("John");
        controller.Start();
        Assert.IsFalse(railroad.Mortgaged);
        Assert.IsFalse(firstBlue.Mortgaged);
        Assert.IsFalse(firstOrange.Mortgaged);
        Assert.IsFalse(secondOrange.Mortgaged);
        Assert.IsFalse(thirdOrange.Mortgaged);
        controller.MoveNext();
        Assert.AreEqual(1590, player.Cash);
        Assert.IsTrue(railroad.Mortgaged);
        Assert.IsFalse(firstBlue.Mortgaged);
        Assert.IsTrue(firstOrange.Mortgaged);
        Assert.IsFalse(secondOrange.Mortgaged);
        Assert.IsFalse(thirdOrange.Mortgaged);
    }

    [TestMethod("Mortgage (2)")]
    public void TestMortgageMortgaged()
    {
        Controller controller = Factory.CreateController(1, 2, 6, 4);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed railroad = game.Deeds[5];
        Deed firstBlue = game.Deeds[37];
        Deed firstOrange = game.Deeds[16];
        Deed secondOrange = game.Deeds[18];

        player.Agent = TestAgent
            .Create()
            .ThenMortgage(17, 6, 19)
            .ThenExpect(Warning.Mortgaged);
        player.SquareId = 36;
        railroad.PlayerId = 1;
        firstBlue.PlayerId = 1;
        firstOrange.PlayerId = 1;
        secondOrange.PlayerId = 1;
        secondOrange.Mortgaged = true;

        controller.AddPlayer("John");
        controller.Start();
        Assert.IsFalse(railroad.Mortgaged);
        Assert.IsFalse(firstBlue.Mortgaged);
        Assert.IsFalse(firstOrange.Mortgaged);
        Assert.IsTrue(secondOrange.Mortgaged);
        controller.MoveNext();
        Assert.AreEqual(1590, player.Cash);
        Assert.IsTrue(railroad.Mortgaged);
        Assert.IsFalse(firstBlue.Mortgaged);
        Assert.IsTrue(firstOrange.Mortgaged);
        Assert.IsTrue(secondOrange.Mortgaged);
    }

    [TestMethod("Mortgage (3)")]
    public void TestMortgageAccessDenied()
    {
        Controller controller = Factory.CreateController(1, 2, 6, 4, 6, 4, 6, 4);
        Game game = controller.Game;
        Player first = controller.AddPlayer("Mark");
        Deed firstOrange = game.Deeds[16];
        Deed secondOrange = game.Deeds[18];
        Deed thirdOrange = game.Deeds[19];
        Deed firstRed = game.Deeds[21];
        Deed secondRed = game.Deeds[23];
        Deed thirdRed = game.Deeds[24];

        first.Agent = TestAgent
            .Create()
            .ThenMortgage(17)
            .ThenExpect(Warning.None)
            .ThenMortgage(22)
            .ThenExpect(Warning.AccessDenied);
        first.SquareId = 36;
        firstOrange.PlayerId = 1;
        secondOrange.PlayerId = 1;
        thirdOrange.PlayerId = 1;
        firstRed.PlayerId = 2;
        secondRed.PlayerId = 2;
        thirdRed.PlayerId = 3;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        controller.MoveNext();
        Assert.AreEqual(1690, first.Cash);
        Assert.IsTrue(firstOrange.Mortgaged);
        Assert.IsFalse(secondOrange.Mortgaged);
        Assert.IsFalse(thirdOrange.Mortgaged);
        Assert.IsFalse(firstRed.Mortgaged);
        Assert.IsFalse(secondRed.Mortgaged);
        Assert.IsFalse(thirdRed.Mortgaged);
    }

    [TestMethod("Mortgage (4)")]
    public void TestMortgageImproved()
    {
        Controller controller = Factory.CreateController(1, 2, 6, 4);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed deed = game.Deeds[16];

        player.Agent = TestAgent
            .Create()
            .ThenMortgage(17)
            .ThenExpect(Warning.Improved);
        player.SquareId = 36;
        deed.PlayerId = 1;
        deed.Improvements = 3;
        game.Houses -= 3;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(1400, player.Cash);
        Assert.IsFalse(deed.Mortgaged);
    }

    [TestMethod("Unmortgage (1)")]
    public void TestUnmortgage()
    {
        Controller controller = Factory.CreateController(4, 6, 6, 4);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed secondOrange = game.Deeds[18];
        Deed thirdOrange = game.Deeds[19];

        player.Agent = TestAgent
            .Create()
            .ThenUnmortgage(19, 20);
        secondOrange.PlayerId = 1;
        secondOrange.Mortgaged = true;
        thirdOrange.PlayerId = 1;
        thirdOrange.Mortgaged = true;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(1291, player.Cash);
        Assert.IsFalse(secondOrange.Mortgaged);
        Assert.IsFalse(thirdOrange.Mortgaged);
    }

    [TestMethod("Unmortgage (2)")]
    public void TestUnmortgageAccessDenied()
    {
        Controller controller = Factory.CreateController(4, 6, 6, 4);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed secondOrange = game.Deeds[18];
        Deed thirdOrange = game.Deeds[19];

        player.Agent = TestAgent
            .Create()
            .ThenUnmortgage(19, 20)
            .ThenExpect(Warning.AccessDenied);
        secondOrange.PlayerId = 1;
        secondOrange.Mortgaged = true;
        thirdOrange.PlayerId = 2;
        thirdOrange.Mortgaged = true;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(1401, player.Cash);
        Assert.IsFalse(secondOrange.Mortgaged);
        Assert.IsTrue(thirdOrange.Mortgaged);
    }

    [TestMethod("Unmortgage (3)")]
    public void TestUnmortgageUnmortgaged()
    {
        Controller controller = Factory.CreateController(4, 6, 6, 4);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed secondOrange = game.Deeds[18];
        Deed thirdOrange = game.Deeds[19];

        player.Agent = TestAgent
            .Create()
            .ThenUnmortgage(19, 20)
            .ThenExpect(Warning.Unmortgaged);
        secondOrange.PlayerId = 1;
        secondOrange.Mortgaged = true;
        thirdOrange.PlayerId = 1;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(1401, player.Cash);
        Assert.IsFalse(secondOrange.Mortgaged);
        Assert.IsFalse(thirdOrange.Mortgaged);
    }

    [TestMethod("Unmortgage (4)")]
    public void TestUnmortgageInsufficientCash()
    {
        Controller controller = Factory.CreateController(4, 6, 6, 4);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed secondOrange = game.Deeds[18];
        Deed thirdOrange = game.Deeds[19];

        player.Agent = TestAgent
            .Create()
            .ThenUnmortgage(19, 20)
            .ThenExpect(Warning.InsufficientCash);
        player.Cash = 200;
        secondOrange.PlayerId = 1;
        secondOrange.Mortgaged = true;
        thirdOrange.PlayerId = 1;
        thirdOrange.Mortgaged = true;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(101, player.Cash);
        Assert.IsFalse(secondOrange.Mortgaged);
        Assert.IsTrue(thirdOrange.Mortgaged);
    }

    [TestMethod("Improve (1)")]
    public void TestImprove()
    {
        Controller controller = Factory.CreateController(4, 1, 6, 4);
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
        Controller controller = Factory.CreateController(4, 1, 6, 4);
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
        Controller controller = Factory.CreateController(4, 1, 6, 4);
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
        Controller controller = Factory.CreateController(4, 1, 6, 4);
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
        Controller controller = Factory.CreateController(4, 1, 6, 4);
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
        Controller controller = Factory.CreateController(4, 1, 6, 4);
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
        Controller controller = Factory.CreateController(4, 1, 4, 1);
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

    [TestMethod("Unimprove (1)")]
    public void TestUnimprove()
    {
        Controller controller = Factory.CreateController(1, 3, 6, 4);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed firstPink = game.Deeds[11];
        Deed secondPink = game.Deeds[13];
        Deed thirdPink = game.Deeds[14];

        player.Agent = TestAgent
            .Create()
            .ThenUnimprove(12, 14, 15, 12, 14);
        firstPink.PlayerId = 1;
        firstPink.Improvements = 4;
        secondPink.PlayerId = 1;
        secondPink.Improvements = 4;
        thirdPink.PlayerId = 1;
        thirdPink.Improvements = 4;
        game.Houses -= 12;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(2, firstPink.Improvements);
        Assert.AreEqual(2, secondPink.Improvements);
        Assert.AreEqual(3, thirdPink.Improvements);
        Assert.AreEqual(25, game.Houses);
        Assert.AreEqual(12, game.Hotels);
        Assert.AreEqual(1550, player.Cash);
    }

    [TestMethod("Unimprove (2)")]
    public void TestUnimproveUnimproved()
    {
        Controller controller = Factory.CreateController(1, 3, 6, 4);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed firstPink = game.Deeds[11];
        Deed secondPink = game.Deeds[13];
        Deed thirdPink = game.Deeds[14];

        player.Agent = TestAgent
            .Create()
            .ThenUnimprove(12, 14, 15, 12, 14, 15, 12, 14, 15, 12, 14, 15, 12, 14, 15)
            .ThenExpect(Warning.Unimproved);
        firstPink.PlayerId = 1;
        firstPink.Improvements = 4;
        secondPink.PlayerId = 1;
        secondPink.Improvements = 4;
        thirdPink.PlayerId = 1;
        thirdPink.Improvements = 4;
        game.Houses -= 12;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(0, firstPink.Improvements);
        Assert.AreEqual(0, secondPink.Improvements);
        Assert.AreEqual(0, thirdPink.Improvements);
        Assert.AreEqual(32, game.Houses);
        Assert.AreEqual(12, game.Hotels);
        Assert.AreEqual(1900, player.Cash);
    }

    [TestMethod("Unimprove (3)")]
    public void TestUnimproveAccessDenied()
    {
        Controller controller = Factory.CreateController(1, 3, 6, 4);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed firstPink = game.Deeds[11];
        Deed secondPink = game.Deeds[13];
        Deed thirdPink = game.Deeds[14];

        player.Agent = TestAgent
            .Create()
            .ThenUnimprove(12, 14, 15, 12, 14, 15)
            .ThenExpect(Warning.AccessDenied);
            
        firstPink.PlayerId = 2;
        firstPink.Improvements = 4;
        secondPink.PlayerId = 2;
        secondPink.Improvements = 4;
        thirdPink.PlayerId = 2;
        thirdPink.Improvements = 4;
        game.Houses -= 12;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(4, firstPink.Improvements);
        Assert.AreEqual(4, secondPink.Improvements);
        Assert.AreEqual(4, thirdPink.Improvements);
        Assert.AreEqual(20, game.Houses);
        Assert.AreEqual(12, game.Hotels);
        Assert.AreEqual(1300, player.Cash);
    }

    [TestMethod("Offer (1)")]
    public void TestOffer()
    {
        Controller controller = Factory.CreateController(3, 3, 1, 1, 6, 4, 6, 4, 1, 2);
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
        Controller controller = Factory.CreateController(3, 3, 1, 1, 6, 4, 6, 4, 1, 2);
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
        Controller controller = Factory.CreateController(4, 6, 4, 6);
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
        Controller controller = Factory.CreateController(4, 6, 4, 6);
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
        Controller controller = Factory.CreateController(4, 6, 4, 6);
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
        Controller controller = Factory.CreateController(4, 6, 4, 6);
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

    [TestMethod("Unpolice (1)")]
    public void TestUnpolice()
    {
        Controller controller = Factory.CreateController(2, 3, 6, 4, 1, 4, 6, 4, 6, 4, 6, 4);
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

    [TestMethod("Unpolice (2)")]
    public void TestUnpoliceNone()
    {
        Controller controller = Factory.CreateController(2, 3, 6, 4, 1, 4, 6, 4, 5, 5, 6, 4);
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

    [TestMethod("Unpolice (3)")]
    public void TestUnpoliceBail()
    {
        Controller controller = Factory.CreateController(3, 5, 6, 4);
        Player player = controller.AddPlayer("Mark");

        player.Agent = TestAgent
            .Create()
            .ThenUnpolice(UnpoliceStrategy.Bail);
        player.SquareId = 11;
        player.Sentence = 3;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(19, player.SquareId);
        Assert.AreEqual(0, player.Sentence);
        Assert.AreEqual(1450, player.Cash);
    }

    [TestMethod("Unpolice (4)")]
    public void TestUnpoliceCard()
    {
        Controller controller = Factory.CreateController(3, 5, 6, 4);
        Player player = controller.AddPlayer("Mark");

        player.Agent = TestAgent
            .Create()
            .ThenUnpolice(UnpoliceStrategy.Card);
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

    [TestMethod("Unpolice (5)")]
    public void TestUnpoliceInsufficientCards()
    {
        Controller controller = Factory.CreateController(3, 5, 6, 4);
        Player player = controller.AddPlayer("Mark");

        player.Agent = TestAgent
            .Create()
            .ThenUnpolice(UnpoliceStrategy.Card)
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

    [TestMethod("Unpolice (6)")]
    public void TestUnpolicePolice()
    {
        Controller controller = Factory.CreateController(5, 5, 6, 4, 6, 4, 6, 4);
        Player player = controller.AddPlayer("Mark");

        player.Agent = TestAgent
            .Create()
            .ThenUnpolice(UnpoliceStrategy.Bail);
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
        Controller controller = Factory.CreateController(2, 2, 3, 3, 5, 5, 6, 4);
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
        Controller controller = Factory.CreateController(4, 6, 4, 6, 4, 6, 4, 6, 4, 6, 4, 6, 4, 6, 4, 6, 4, 6, 4, 6, 4, 6);
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
