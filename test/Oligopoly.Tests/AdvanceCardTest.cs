using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Assets;
using Oligopoly.Cards;

namespace Oligopoly.Tests;

[TestClass]
public class AdvanceCardTest
{
    [TestMethod("Draw")]
    public void TestDraw()
    {
        Board board = Factory.CreateBoard();
        Game game = Factory.CreateGame(board);
        GameController controller = new GameController(board, game);
        Player player = controller.AddPlayer("Mark");

        player.SquareId = 18;

        AdvanceCard card = new AdvanceCard("Advance to Free Parking", squareId: 21)
        {
            Id = new CardId(id: 1, deckId: 1)
        };

        card.Draw(player, controller);
        Assert.AreEqual(21, player.SquareId);
        Assert.AreEqual(1500, player.Cash);
        card.Draw(player, controller);
        Assert.AreEqual(21, player.SquareId);
        Assert.AreEqual(1500, player.Cash);

        player.SquareId = 23;

        card.Draw(player, controller);
        Assert.AreEqual(21, player.SquareId);
        Assert.AreEqual(1700, player.Cash);

        card = new AdvanceCard("Advance to GO", squareId: 1)
        {
            Id = new CardId(id: 2, deckId: 1)
        };

        card.Draw(player, controller);
        Assert.AreEqual(1, player.SquareId);
        Assert.AreEqual(1900, player.Cash);
    }
}
