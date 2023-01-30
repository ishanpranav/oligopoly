using System;
using System.Diagnostics;
using Oligopoly.Agents;

namespace Oligopoly;

public class Player
{
    private readonly Agent _agent;
    private readonly Board _board;

    private TimeSpan _elapsed;
    private TimeSpan _remaining;

    public Player(int id, Agent agent, Board board)
    {
        ArgumentNullException.ThrowIfNull(agent);
        ArgumentNullException.ThrowIfNull(board);
    }

    public void Execute(Action<Agent> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        Stopwatch stopwatch = Stopwatch.StartNew();

        action(_agent);
        stopwatch.Stop();

        _elapsed += stopwatch.Elapsed;
        _remaining -= stopwatch.Elapsed;
    }
}
