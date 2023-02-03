using System;
using System.Collections.Generic;
using MessagePack;

namespace Oligopoly;

[MessagePackObject]
public partial class Game
{
    private int _turn;
    
    public Game(IReadOnlyList<Player> players, DeckCollection decks)
    {
        ArgumentNullException.ThrowIfNull(players);
        ArgumentNullException.ThrowIfNull(decks);

        Players = players;
        Decks = decks;
    }

    [IgnoreMember]
    public Player Current
    {
        get
        {
            int id = Turn % Players.Count;

            if (id >= Players.Count)
            {
                throw new InvalidOperationException();
            }

            return Players[id];
        }
    }

    [Key(0)]
    public IReadOnlyList<Player> Players { get; }

    [Key(1)]
    public DeckCollection Decks { get; }

    [Key(2)]
    public int Turn
    {
        get
        {
            return _turn;
        }
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            _turn = value;
        }
    }

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
