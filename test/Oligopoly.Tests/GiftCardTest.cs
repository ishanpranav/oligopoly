using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Assets;
using Oligopoly.Cards;

namespace Oligopoly.Tests;

[TestClass]
public class GiftCardTest
{
    [TestMethod("Draw")]
    public void TestDraw()
    {
        Controller controller = Factory.CreateController();
        Player first = controller.AddPlayer("Mark");
        Player second = controller.AddPlayer("John");
        Player third = controller.AddPlayer("Allison");
        Player fourth = controller.AddPlayer("Elizabeth");
        GiftCard card = new GiftCard(nameof(GiftCard), 25)
        {
            Id = new CardId(id: 1, deckId: 1)
        };

        card.Draw(second, controller);
        Assert.AreEqual(1525, first.Cash);
        Assert.AreEqual(1425, second.Cash);
        Assert.AreEqual(1525, third.Cash);
        Assert.AreEqual(1525, fourth.Cash);
    }
}
