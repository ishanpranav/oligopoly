using System;
using System.Collections.Generic;
using Oligopoly.Squares;
using Oligopoly.Writers;

namespace Oligopoly;

public class Board : IWritable
{
    public Board(int railroadCost, IReadOnlyCollection<Square> squares, IReadOnlyList<int> railroadRents, IReadOnlyCollection<Group> groups)
    {
        if (railroadCost <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(railroadCost));
        }

        ArgumentNullException.ThrowIfNull(squares);
        ArgumentNullException.ThrowIfNull(railroadRents);

        if (railroadRents.Count < 1)
        {
            throw new ArgumentException(string.Empty, nameof(railroadRents));
        }

        foreach (int railroadRent in railroadRents)
        {
            if (railroadRent <= 0)
            {
                throw new ArgumentException(string.Empty, nameof(railroadRents));
            }
        }

        ArgumentNullException.ThrowIfNull(groups);

        RailroadCost = railroadCost;
        Squares = squares;
        RailroadRents = railroadRents;
        Groups = groups;
    }

    public int RailroadCost { get; }
    public IReadOnlyCollection<Square> Squares { get; }
    public IReadOnlyList<int> RailroadRents { get; }
    public IReadOnlyCollection<Group> Groups { get; }

    /// <inheritdoc/>
    public void Write(Writer writer)
    {
        writer.Write(RailroadCost);
        writer.Write(Squares);

        foreach (int railroadRent in RailroadRents)
        {
            writer.Write(railroadRent);
        }

        writer.Write(Groups);
    }
}
