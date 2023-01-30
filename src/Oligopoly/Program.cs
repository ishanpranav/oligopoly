using System.IO;
using System.Text.Json;
using MessagePack;
using Oligopoly.Agents;

namespace Oligopoly;

internal static class Program
{
    private static void Main()
    {
        Board board;
        string jsonPath = "../../../../../data/board.json";
        string msgpackPath = "../../../../../data/board.msgpack";
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

        Agent agent = new Agent();
        Game game = new Game(board);
        GameController controller = new GameController(game);

        game.Add("Mark", agent);
        game.Add("Jacob", agent);
        game.Add("Alexander", agent);
        //controller.Start();
    }
}
