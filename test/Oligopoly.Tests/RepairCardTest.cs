using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Assets;
using Oligopoly.Cards;

namespace Oligopoly.Tests;

[TestClass]
public class RepairCardTest
{
    [TestMethod("Draw")]
    public void TestDraw()
    {
        Controller controller = Factory.CreateController();
        Game game = controller.Game;
        Player player = controller.AddPlayer("Mark");
        Deed firstBlue = game.Deeds[37];
        Deed secondBlue = game.Deeds[39];
        Deed firstPink = game.Deeds[11];
        Deed secondPink = game.Deeds[13];
        Deed thirdPink = game.Deeds[14];
        Deed firstBrown = game.Deeds[1];
        Deed secondBrown = game.Deeds[3];

        firstBlue.PlayerId = 1;
        secondBlue.PlayerId = 1;
        secondBlue.Improvements = 1;
        firstPink.PlayerId = 1;
        firstPink.Improvements = 2;
        secondPink.PlayerId = 1;
        secondPink.Improvements = 3;
        thirdPink.PlayerId = 1;
        thirdPink.Improvements = 3;
        firstBrown.PlayerId = 1;
        firstBrown.Improvements = 4;
        secondBrown.PlayerId = 1;
        secondBrown.Improvements = 5;
        game.Houses -= 13;
        game.Hotels -= 1;

        RepairCard card = new RepairCard(nameof(RepairCard), houseCost: 50, hotelCost: 125)
        {
            Id = new CardId(id: 1, deckId: 1)
        };

        card.Draw(player, controller);

        Assert.AreEqual(725, player.Cash);
    }
}
