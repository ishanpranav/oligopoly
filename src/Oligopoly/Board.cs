using System;
using System.Collections.Generic;
using Oligopoly.Squares;
using Oligopoly.Writers;

namespace Oligopoly;

public class Board : IWritable
{
    private readonly Game _game;
    private readonly IReadOnlyList<Square> _squares;
    private readonly IReadOnlyCollection<Group> _groups;

    public Board(Game game, IReadOnlyList<Square> squares, IReadOnlyCollection<Group> groups)
    {
        ArgumentNullException.ThrowIfNull(game);
        ArgumentNullException.ThrowIfNull(squares);
        ArgumentNullException.ThrowIfNull(groups);

        _game = game;
        _squares = squares;
        _groups = groups;
    }

    /// <inheritdoc/>
    public void Write(Writer writer)
    {
        writer.Write(_groups);
        writer.Write(_squares);

        IEnumerable<int>? utilityRents = null;
        IEnumerable<int>? railroadRents = null;

        foreach (Square square in _squares)
        {
            if (utilityRents is not null && railroadRents is not null)
            {
                break;
            }

            if (square is UtilitySquare utilitySquare)
            {
                utilityRents = utilitySquare.Rents;
            }

            if (square is RailroadSquare railroadSquare)
            {
                railroadRents = railroadSquare.Rents;
            }
        }

        if (utilityRents is not null)
        {
            foreach (int utilityRent in utilityRents)
            {
                writer.Write(utilityRent);
            }
        }

        if (railroadRents is not null)
        {
            foreach (int railroadRent in railroadRents)
            {
                writer.Write(railroadRent);
            }
        }
    }
}
