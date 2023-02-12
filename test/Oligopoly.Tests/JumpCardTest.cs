using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Assets;
using Oligopoly.Cards;

namespace Oligopoly.Tests;

[TestClass]
public class JumpCardTest
{
    [TestMethod("Draw")]
    public void TestDraw()
    {
        GameController controller = Factory.CreateController();
        Player player = controller.AddPlayer("Mark");
        JumpCard card = new JumpCard(nameof(JumpCard), distance: -3)
        {
            Id = new CardId(id: 1, deckId: 1)
        };

        player.SquareId = 8;

        card.Draw(player, controller);
        Assert.AreEqual(5, player.SquareId);
        Assert.AreEqual(1300, player.Cash);

        player.SquareId = 2;

        card.Draw(player, controller);
        Assert.AreEqual(39, player.SquareId);
        Assert.AreEqual(1200, player.Cash);
    }
}
