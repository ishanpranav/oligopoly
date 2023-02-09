using System.IO;
using System.Text.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Dice;
using Oligopoly.Shuffles;

namespace Oligopoly.Tests;

[TestClass]
public class Factory
{
    public static Board CreateBoard()
    {
        string messagePackPath = "../../../../../data/board.bin";

        if (File.Exists(messagePackPath))
        {
            using FileStream input = File.OpenRead(messagePackPath);

            return OligopolySerializer.ReadBoard(input);
        }
        else
        {
            using FileStream input = File.OpenRead("../../../../../data/board.json");

            Board? board = JsonSerializer.Deserialize<Board>(input, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsNotNull(board);

            using FileStream output = File.Create(messagePackPath);

            OligopolySerializer.Write(output, board);

            return board;
        }
    }

    public static Game CreateGame(Board board, IDice? dice = null, IShuffle? shuffle = null)
    {
        if (dice is null)
        {
            dice = new D6PairDice();
        }

        if (shuffle is null)
        {
            shuffle = new FisherYatesShuffle();
        }

        return new Game(board.Squares, board.Decks, dice, shuffle)
        {
            Houses = board.Houses,
            Hotels = board.Hotels
        };
    }
}
