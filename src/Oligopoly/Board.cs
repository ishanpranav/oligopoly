using System;
using System.Collections.Generic;
using Oligopoly.Squares;
using Oligopoly.Writers;

namespace Oligopoly;

public class Board : IWritable
{
    public Board(BoardSettings settings, IReadOnlyCollection<Square> squares, IReadOnlyCollection<Group> groups)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(squares);
        ArgumentNullException.ThrowIfNull(groups);

        Settings = settings;
        Squares = squares;
        Groups = groups;
    }

    public BoardSettings Settings { get; }
    public IReadOnlyCollection<Square> Squares { get; }
    public IReadOnlyCollection<Group> Groups { get; }

    /// <inheritdoc/>
    public void Write(Writer writer)
    {
        writer.Write(Settings);

        int utilityCost = 0;
        int railroadCost = 0;
        IEnumerable<int>? utilityRents = null;
        IEnumerable<int>? railroadRents = null;
        
        foreach (Square square in Squares)
        {
            if (utilityCost is not 0 && railroadCost is not 0)
            {
                break;
            }

            if (square is UtilitySquare utilitySquare)
            {
                utilityCost = utilitySquare.Cost;
                utilityRents = utilitySquare.Rents;
            }

            if (square is RailroadSquare railroadSquare)
            {
                railroadCost = railroadSquare.Cost;
                railroadRents = railroadSquare.Rents;
            }
        }

        writer.Write(utilityCost);
        writer.Write(railroadCost);
        writer.Write(Squares);

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

        writer.Write(Groups);
    }
}
