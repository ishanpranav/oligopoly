using System;
using System.IO;
using System.Text.Json;
using MessagePack;
using Oligopoly.Agents;

namespace Oligopoly;

internal static class Program
{
    private static void Main()
    {
        const string jsonPath = "../../../../../data/board.json";
        const string msgpackPath = "../../../../../data/board.msgpack";
        const string gamePath = "../../../../../data/game.msgpack";

        Game game;
        Board board;
        MessagePackSerializerOptions msgpackOptions = MessagePackSerializerOptions.Standard
            .WithCompression(MessagePackCompression.Lz4Block)
            .WithSecurity(MessagePackSecurity.UntrustedData);

        if (File.Exists(msgpackPath))
        {
            using Stream input = File.OpenRead(msgpackPath);

            board = MessagePackSerializer.Deserialize<Board>(input, msgpackOptions);
        }
        else
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReadCommentHandling = JsonCommentHandling.Skip,
                WriteIndented = true
            };

            using Stream input = File.OpenRead(jsonPath);

            board = JsonSerializer.Deserialize<Board>(input, options)!;

            using Stream output = File.Create(msgpackPath);

            MessagePackSerializer.Serialize(output, board, msgpackOptions);
        }

        Agent agent = Agent.Default;

        if (File.Exists(gamePath))
        {
            using Stream input = File.OpenRead(gamePath);

            game = MessagePackSerializer.Deserialize<Game>(input, msgpackOptions);

            foreach (Player player in game.Players)
            {
                player.Agent = agent;
            }
        }
        else
        {
            DeckCollection decks = new DeckCollection(board.Decks);

            decks.Shuffle();

            game = new Game(new Player[]
            {
                new Player(1, "Mark") { Agent = agent, Cash = board.InitialCash  },
                new Player(2, "Jacob") { Agent = agent, Cash = board.InitialCash },
                new Player(3, "Alexander") { Agent = agent, Cash = board.InitialCash }
            }, decks);
        }

        GameController controller = new GameController(game);

        controller.Start();

        while (controller.MoveNext())
        {
            Console.ReadLine();
        }

        using Stream gameOutput = File.Create(gamePath);

        MessagePackSerializer.Serialize(gameOutput, game, msgpackOptions);
    }
}
