using System.Collections.Generic;
using Oligopoly.Agents;
using Oligopoly.Assets;

namespace Oligopoly;

public class Player
{
    private readonly Board _board;

    internal Player(string name, Agent agent, Board board)
    {
        Name = name;
        Agent = agent;
        _board = board;
    }

    public string Name { get; }
    public Agent Agent { get; }
    public Portfolio Portfolio { get; } = new Portfolio();

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
