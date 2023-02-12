using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Assets;

namespace Oligopoly.Tests;

[TestClass]
public class StreetSquareTest
{
    [DataRow(21, 2, 5, 1478, 1522, 28, DisplayName = "Advance (1)")]
    [DataRow(26, 4, 5, 1444, 1556, 32, 33, 35, DisplayName = "Advance (2)")]
    [DataTestMethod]
    public void TestAdvance(int startSquare, int first, int second, int playerCash, int ownerCash, params int[] squareIds)
    {
        Controller controller = Factory.CreateController(first, second, 6, 4);
        Player player = controller.AddPlayer("Mark");
        Player owner = controller.AddPlayer("John");

        player.SquareId = startSquare;

        foreach (int squareId in squareIds)
        {
            controller.Game.Deeds[squareId - 1].PlayerId = 2;
        }

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(startSquare + first + second, player.SquareId);
        Assert.AreEqual(playerCash, player.Cash);
        Assert.AreEqual(ownerCash, owner.Cash);
    }

    [TestMethod("Advance (3)")]
    public void TestAdvanceImproved()
    {
        Controller controller = Factory.CreateController(1, 3, 4, 6);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Player owner = controller.AddPlayer("John");
        Deed deed = game.Deeds[24];

        player.SquareId = 21;
        deed.PlayerId = 2;
        deed.Improvements = 3;
        game.Houses -= 3;

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(25, player.SquareId);
        Assert.AreEqual(750, player.Cash);
        Assert.AreEqual(2250, owner.Cash);
    }

    [TestMethod("Advance (4)")]
    public void TestAdvanceInsufficientCash()
    {
        Controller controller = Factory.CreateController(1, 3, 4, 6);
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Player owner = controller.AddPlayer("John");
        Deed deed = game.Deeds[39];

        player.SquareId = 36;
        deed.PlayerId = 2;
        deed.Improvements = 5;
        game.Houses -= 5;

        controller.Start();
        controller.MoveNext();
        Assert.IsFalse(game.Players.Contains(player));
        Assert.AreEqual(40, player.SquareId);
        Assert.AreEqual(-500, player.Cash);
        Assert.AreEqual(3000, owner.Cash);
    }
}
