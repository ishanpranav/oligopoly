using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Assets;
using Oligopoly.Cards;

namespace Oligopoly.Tests;

[TestClass]
public class PoliceCardTest
{
    [TestMethod("Draw")]
    public void TestDraw()
    {
        GameController controller = Factory.CreateController();
        Player player = controller.AddPlayer("Mark");
        PoliceCard card = new PoliceCard(nameof(PoliceCard))
        {
            Id = new CardId(id: 1, deckId: 1)
        };

        player.SquareId = 18;

        card.Draw(player, controller);
        Assert.AreEqual(11, player.SquareId);
        Assert.AreEqual(3, player.Sentence);
    }
}
