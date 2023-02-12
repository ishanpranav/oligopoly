using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Assets;
using Oligopoly.Cards;

namespace Oligopoly.Tests;

[TestClass]
public class UngiftCardTest
{
    [TestMethod("Draw")]
    public void TestDraw()
    {
        GameController controller = Factory.CreateController();
        Game game = controller.Game;
        Player first = controller.AddPlayer("Mark");
        Player second = controller.AddPlayer("John");
        Player third = controller.AddPlayer("Allison");
        UngiftCard card = new UngiftCard(nameof(UngiftCard), amount: 20)
        {
            Id = new CardId(id: 1, deckId: 1)
        };

        card.Draw(first, controller);
        Assert.AreEqual(1540, first.Cash);
        Assert.AreEqual(1480, second.Cash);
        Assert.AreEqual(1480, third.Cash);
    }
}
