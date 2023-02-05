using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using MessagePack;

namespace Oligopoly;

internal static class Program
{
    private static int s_id;
    private static Board? s_board;

    private static void Main()
    {
        const string boardSourcePath = "../../../../../data/board.json";
        const string boardPath = "../../../../../data/board.bin";
        const string gamePath = "../../../../../data/game.bin";
        const string gameSourcePath = "../../../../../data/game.json";

        Game game;
        MessagePackSerializerOptions messagePackOptions = MessagePackSerializerOptions.Standard
            .WithCompression(MessagePackCompression.Lz4Block)
            .WithSecurity(MessagePackSecurity.UntrustedData);
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

            s_board = MessagePackSerializer.Deserialize<Board>(input, messagePackOptions);
        }
        else
        {
            using Stream input = File.OpenRead(boardSourcePath);

            s_board = JsonSerializer.Deserialize<Board>(input, jsonOptions)!;

            using Stream output = File.Create(boardPath);

            MessagePackSerializer.Serialize(output, s_board, messagePackOptions);
        }

        if (File.Exists(gamePath))
        {
            using Stream input = File.OpenRead(gamePath);

            game = MessagePackSerializer.Deserialize<Game>(input, messagePackOptions);
        }
        else
        {
            game = new Game(new Player[]
            {
                CreatePlayer("Mark"),
                CreatePlayer("Jacob"),
                CreatePlayer("Alexander")
            }, s_board.Squares, s_board.Decks);
        }

        GameController controller = new GameController(s_board, game);

        controller.Start();

        while (controller.MoveNext())
        {
            using (Stream output = File.Create(gamePath))
            {
                MessagePackSerializer.Serialize(output, game, messagePackOptions);
            }

            using (Stream output = File.Create(gameSourcePath))
            {
                JsonSerializer.Serialize(output, game, jsonOptions);
            }

            Console.ReadLine();
        }
    }

    private static Player CreatePlayer(string name)
    {
        s_id++;

        return new Player(s_id, name)
        {
            Cash = s_board!.Savings
        };
    }
}
