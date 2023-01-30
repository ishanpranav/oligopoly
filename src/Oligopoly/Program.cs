using System.IO;
using System.Linq;
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
        MessagePackSerializerOptions msgpackOptions = MessagePackSerializerOptions.Standard.WithCompression(MessagePackCompression.Lz4Block);

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
            using Stream output = File.Create(msgpackPath);

            board = JsonSerializer.Deserialize<Board>(input, options)!;

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
            game = new Game(new Player[]
            {
                new Player("Mark") { Agent = agent },
                new Player("Jacob") { Agent = agent },
                new Player("Alexander") { Agent = agent }
            }, Enumerable.Empty<Player>());

            using Stream output = File.Create(gamePath);

            MessagePackSerializer.Serialize<Game>(output, game, msgpackOptions);
        }

        GameController controller = new GameController(game);

        controller.Start();
    }
}
