using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Oligopoly.Tests;

[TestClass]
public class PoliceSquareTest
{
    [DataRow(2, 4, 6, 4, DisplayName = "Advance (1)")]
    [DataRow(3, 3, 6, 4, DisplayName = "Advance (2)")]
    [DataTestMethod]
    public void TestAdvance(params int[] items)
    {
        Controller controller = Factory.CreateController(items);
        Player player = controller.AddPlayer("Mark");

        player.SquareId = 25;

        controller.AddPlayer("John");
        controller.Start();
        controller.MoveNext();
        Assert.AreEqual(11, player.SquareId);
        Assert.AreEqual(3, player.Sentence);
        Assert.AreEqual(1500, player.Cash);
    }
}
