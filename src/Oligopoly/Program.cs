using System.IO;

namespace Oligopoly;

internal static class Program
{
    private static void Main()
    {
        using Stream input = File.OpenRead("../../../../../data/board.dat");
        using BinaryReader reader = new BinaryReader(input);

        Game game = reader.ReadGame();

        game.Seed = 1;
    }
}
