using System;
using System.Collections.Generic;
using MessagePack;
using Oligopoly.Squares;

namespace Oligopoly;

[MessagePackObject]
public class Board
{
    public Board(IReadOnlyList<ISquare> squares, IReadOnlyCollection<Group> groups)
    {
        ArgumentNullException.ThrowIfNull(squares);
        ArgumentNullException.ThrowIfNull(groups);

        Squares = squares;
        Groups = groups;
    }

    [Key(0)]
    public IReadOnlyList<ISquare> Squares { get; }

    [Key(1)]
    public IReadOnlyCollection<Group> Groups { get; }
}
