using System.IO;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Dice;
using Oligopoly.Shuffles;

namespace Oligopoly.Tests;

[TestClass]
public class Factory
{
    private static Board? s_board;
    private static D6PairDice? s_dice;
    private static FisherYatesShuffle? s_shuffle;
    
    public static Board CreateBoard()
    {
        if (s_board is null)
        {
            string messagePackPath = "../../../../../data/board.bin";

            if (File.Exists(messagePackPath))
            {
                using FileStream input = File.OpenRead(messagePackPath);

                s_board = OligopolySerializer.ReadBoard(input);
            }
            else
            {
                using FileStream input = File.OpenRead("../../../../../data/board.json");

                s_board = JsonSerializer.Deserialize<Board>(input, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                Assert.IsNotNull(s_board);

                using FileStream output = File.Create(messagePackPath);

                OligopolySerializer.Write(output, s_board);
            }
        }

        return s_board;
    }

    public static Game CreateGame(Board board, IDice? dice = null, IShuffle? shuffle = null)
    {
        if (dice is null)
        {
            if (s_dice is null)
            {
                s_dice = new D6PairDice();
            }

            dice = s_dice;
        }

        if (shuffle is null)
        {
            if (s_shuffle is null)
            {
                s_shuffle = new FisherYatesShuffle();
            }

            shuffle = s_shuffle;
        }

        return new Game(board.Squares, board.Decks, dice, shuffle)
        {
            Houses = board.Houses,
            Hotels = board.Hotels
        };
    }

    public static GameController CreateController()
    {
        Board board = CreateBoard();

        return new GameController(board, CreateGame(board));
    }

    public static GameController CreateController(params int[] items)
    {
        Board board = CreateBoard();

        return new GameController(board, CreateGame(board, new D6PairDice(new TestRandom(items))));
    }
}
