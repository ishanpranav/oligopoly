using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Oligopoly.Tests;

[TestClass]
public class UtilitySquareTest
{
    [DataRow(1488, 1512, 13, DisplayName = "Advance (1)")]
    [DataRow(1470, 1530, 13, 29, DisplayName = "Advance (2)")]
    [DataTestMethod]
    public void TestAdvance(int playerCash, int ownerCash, params int[] squareIds)
    {
        Controller controller = Factory.CreateController(1, 2, 6, 4);
        Player player = controller.AddPlayer("Mark");
        Player owner = controller.AddPlayer("John");

        player.SquareId = 10;

        foreach (int squareId in squareIds)
        {
            controller.Game.Deeds[squareId - 1].PlayerId = 2;
        }

        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(13, player.SquareId);
        Assert.AreEqual(playerCash, player.Cash);
        Assert.AreEqual(ownerCash, owner.Cash);
    }
}
