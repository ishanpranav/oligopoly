using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Oligopoly.Dice;
using Oligopoly.Shuffles;

namespace Oligopoly;

internal static class Program
{
    private static Board? s_board;

    private static void Main()
    {
        const string boardSourcePath = "../../../../../data/board-california.json";
        const string boardPath = "../../../../../data/board-california.bin";
        const string gamePath = "../../../../../data/game.bin";
        const string gameSourcePath = "../../../../../data/game.json";

        Game game;
        JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReadCommentHandling = JsonCommentHandling.Skip,
            WriteIndented = false
        };

        if (File.Exists(boardPath))
        {
            using Stream input = File.OpenRead(boardPath);

            s_board = OligopolySerializer.ReadBoard(input);
        }
        else
        {
            using Stream input = File.OpenRead(boardSourcePath);

            s_board = JsonSerializer.Deserialize<Board>(input, jsonOptions)!;

            using Stream output = File.Create(boardPath);

            OligopolySerializer.Write(output, s_board);
        }

        GameController controller;

        if (File.Exists(gamePath))
        {
            using Stream input = File.OpenRead(gamePath);

            game = OligopolySerializer.ReadGame(input);
            controller = new GameController(s_board, game);
        }
        else
        {
            game = new Game(s_board.Squares, s_board.Decks, new D6PairDice(Random.Shared), new FisherYatesShuffle(Random.Shared))
            {
                Houses = s_board.Houses,
                Hotels = s_board.Hotels
            };
            controller = new GameController(s_board, game);

            controller.AddPlayer("Mark");
            controller.AddPlayer("Jacob");
            controller.AddPlayer("Alexander");
        }

        controller.Start();

        while (controller.MoveNext())
        {
            using (Stream output = File.Create(gamePath))
            {
                OligopolySerializer.Write(output, game);
            }

            using (Stream output = File.Create(gameSourcePath))
            {
                JsonSerializer.Serialize(output, game, jsonOptions);
            }

            Console.ReadLine();
        }
    }
}
