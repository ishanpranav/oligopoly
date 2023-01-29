using System;
using System.IO;

namespace Oligopoly;

internal static class Program
{
    private static void Main(string[] args)
    {
        using Stream input = File.OpenRead("../../../../../data/board.dat");
        using BinaryReader reader = new BinaryReader(input);

        if (reader.ReadUInt16() is not 12004)
        {
            throw new FormatException();
        }

        Board board = Board.Read(reader);
    }
}
