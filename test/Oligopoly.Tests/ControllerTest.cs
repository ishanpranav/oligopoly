using System;
using System.IO;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Dice;
using Oligopoly.Shuffles;

namespace Oligopoly.Tests;

[TestClass]
public class ControllerTest
{
    private static Board? s_board;
    private static FisherYatesShuffle? s_shuffle;

    [ClassInitialize]
    public static void Initialize(TestContext context)
    {
        using FileStream input = File.OpenRead(Path.Combine(context.DeploymentDirectory, "board.json"));

        s_board = JsonSerializer.Deserialize<Board>(input, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
        s_shuffle = new FisherYatesShuffle(Random.Shared);

        Assert.IsNotNull(s_board);
        Assert.AreEqual(40, s_board.Squares.Count);
        Assert.AreEqual(200, s_board.Salary);
    }

    [TestMethod("Salary (1)")]
    public void TestSalary()
    {
        Game game = new Game(s_board!.Squares, s_board!.Decks, new D6PairDice(new TestRandom(5, 6)), s_shuffle!);
        GameController controller = new GameController(s_board!, game);
        Player player = controller.AddPlayer("Mark");

        player.SquareId = 40;

        controller.Start();
        controller.MoveNext();

        Assert.AreEqual(11, player.SquareId);
        Assert.AreEqual(1700, player.Cash);
    }

    [TestMethod("Salary (2)")]
    public void TestSalaryAdvance()
    {
        Game game = new Game(s_board!.Squares, s_board!.Decks, new D6PairDice(new TestRandom(1, 2)), s_shuffle!);
        GameController controller = new GameController(s_board!, game);
        Player player = controller.AddPlayer("Mark");

        player.SquareId = 38;

        controller.Start();
        controller.MoveNext();

        Assert.AreEqual(1, player.SquareId);
        Assert.AreEqual(1700, player.Cash);
    }
}
