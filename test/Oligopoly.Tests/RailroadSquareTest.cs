using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Oligopoly.Tests;

[TestClass]
public class RailroadSquareTest
{
    [DataRow(1475, 1525, 36, DisplayName = "Advance (1)")]
    [DataRow(1450, 1550, 36, 26, DisplayName = "Advance (2)")]
    [DataRow(1400, 1600, 36, 26, 16, DisplayName = "Advance (3)")]
    [DataRow(1300, 1700, 36, 26, 16, 6, DisplayName = "Advance (4)")]
    [DataTestMethod]
    public void TestAdvance(int playerCash, int ownerCash, params int[] squareIds)
    {
        Controller controller = Factory.CreateController(2, 4, 6, 4);
        Player player = controller.AddPlayer("Mark");
        Player owner = controller.AddPlayer("John");

        player.SquareId = 30;

        foreach (int squareId in squareIds)
        {
            controller.Game.Deeds[squareId - 1].PlayerId = 2;
        }

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(36, player.SquareId);
        Assert.AreEqual(playerCash, player.Cash);
        Assert.AreEqual(ownerCash, owner.Cash);
    }
}
