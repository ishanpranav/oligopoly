using System;
using System.Diagnostics;
using Oligopoly.Agents;

namespace Oligopoly;

public class Player
{
    private readonly int _id;
    private readonly Board _board;

    public Player(int id, Agent agent, Board board)
    {
        ArgumentNullException.ThrowIfNull(agent);
        ArgumentNullException.ThrowIfNull(board);

        _id = id;
        Agent = agent;
        _board = board;
    }

    public Agent Agent { get; }
}
