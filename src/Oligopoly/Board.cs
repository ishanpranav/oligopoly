using System;
using System.Collections.Generic;
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

    public Board(int squares, int groups)
    {
        if (squares is 0)
        {
            Squares = Array.Empty<Square>();
        }
        else
        {
            Squares = new Square[squares];
        }

        if (groups is 0)
        {
            Groups = Array.Empty<Group>();
        }
        else
        {
            Groups = new Group[groups];
        }
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
}
