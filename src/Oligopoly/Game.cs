using System;
using System.Collections.Generic;
using MessagePack;

namespace Oligopoly;

[MessagePackObject]
public class Game
{
    private readonly List<Player> _players;
    private readonly List<Player> _observers;

    public Game(IEnumerable<Player> players, IEnumerable<Player> observers)
    {
        ArgumentNullException.ThrowIfNull(players);
        ArgumentNullException.ThrowIfNull(observers);

        _players = new List<Player>(players);
        _observers = new List<Player>(observers);
    }

    [IgnoreMember]
    public bool Terminated
    {
        get
        {
            return _players.Count <= 1;
        }
    }

    [Key(1)]
    public IReadOnlyCollection<Player> Players
    {
        get
        {
            return _players;
        }
    }

    [Key(2)]
    public IReadOnlyCollection<Player> Observers
    {
        get
        {
            return _observers;
        }
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
