using System;
using System.Collections.Generic;
using Oligopoly.Agents;

namespace Oligopoly;

public class Game
{
    private readonly Board _board;
    private readonly List<Player> _players = new List<Player>();
    private readonly List<Player> _observers = new List<Player>();

    public Game(Board board)
    {
        ArgumentNullException.ThrowIfNull(board);

        _board = board;
    }

    public bool Terminated
    {
        get
        {
            return _players.Count <= 1;
        }
    }

    public IReadOnlyCollection<Player> Players
    {
        get
        {
            return _players;
        }
    }

    internal void Add(string name, Agent agent)
    {
        _players.Add(new Player(name, agent, _board));
    }

    //public event EventHandler? Started;
    //public event EventHandler? TurnStarted; // Player
    //public event EventHandler? TurnEnded;   // Player
    //public event EventHandler? Moved; // Player
    //public event EventHandler? Charging; // Player, Amount
    //public event EventHandler? Charged;  // Player, Amount
    //public event EventHandler? Paid; // Player, Amount
    //public event EventHandler? JailEscapeAcquired;
    //public event EventHandler? AuctionSucceded; // Property, Player (winner), Amount (bid)
    //public event EventHandler? AuctionFailed; // Property
    //public event EventHandler? DealResponded; // Outcome
    //public event EventHandler? DealClosed; // Deal
    //public event EventHandler? Bankrupted; // Player
    //public event EventHandler? Ended; // Player (winner), Amount (rounds)
    //public event EventHandler? Error; // Message
}
