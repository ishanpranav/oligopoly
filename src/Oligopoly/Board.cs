using System;
using Oligopoly.Readers;
using Oligopoly.Writers;

namespace Oligopoly;

public class Board : IWritable
{
    private readonly Square[] _squares;
    private readonly Group[] _groups;

    public Board()
    {
        _squares = Array.Empty<Square>();
        _groups = Array.Empty<Group>();
    }

    public Board(int squares, int groups)
    {
        if (squares is 0)
        {
            _squares = Array.Empty<Square>();
        }
        else
        {
            _squares = new Square[squares];
        }

        if (groups is 0)
        {
            _groups = Array.Empty<Group>();
        }
        else
        {
            _groups = new Group[groups];
        }
    }

    private Board(Square[] squares, Group[] groups)
    {
        _squares = squares;
        _groups = groups;
    }

    public int Squares
    {
        get
        {
            return _squares.Length;
        }
    }

    public int Groups
    {
        get
        {
            return _groups.Length;
        }
    }

    /// <inheritdoc/>
    public void Write(Writer writer)
    {
        writer.Write(_squares);
        writer.Write(_groups);
    }

    public static Board Deserialize(Reader reader)
    {
        return new Board(
            reader.ReadArray(reader.ReadSquare),
            reader.ReadArray(reader.ReadGroup));
    }
}
