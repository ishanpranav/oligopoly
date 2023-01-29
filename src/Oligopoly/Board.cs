using System;
using System.Collections.Generic;
using System.IO;
using Oligopoly.Squares;
using Oligopoly.Writers;

namespace Oligopoly;

public class Board : IWritable
{
    public Board()
    {
        Squares = Array.Empty<Square>();
        Groups = Array.Empty<Group>();
    }

    public Board(IReadOnlyCollection<Square> squares, IReadOnlyCollection<Group> groups)
    {
        ArgumentNullException.ThrowIfNull(squares);
        ArgumentNullException.ThrowIfNull(groups);

        Squares = squares;
        Groups = groups;
    }

    public IReadOnlyCollection<Square> Squares { get; }
    public IReadOnlyCollection<Group> Groups { get; }

    /// <inheritdoc/>
    public void Write(Writer writer)
    {
        writer.Write(Squares);
        writer.Write(Groups);
    }

    internal static Board Read(BinaryReader reader)
    {
        int length = reader.ReadInt32();
        Square[] squares = new Square[length];

        for (int i = 0; i < length; i++)
        {
            squares[i] = Square.Read(reader);
        }

        length = reader.ReadInt32();

        Group[] groups = Array.Empty<Group>();

        return new Board(squares, groups);
    }
}
