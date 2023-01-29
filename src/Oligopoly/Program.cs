using System.IO;

namespace Oligopoly;

internal static class Program
{
    private static void Main(string[] args)
    {
        using Stream input = File.OpenRead("../../../../../data/board.dat");
        using BinaryReader reader = new BinaryReader(input);

        Board board = reader.ReadBoard();
    }
}
