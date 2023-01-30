using System;
using System.Collections.Generic;
using Oligopoly.Squares;
using Oligopoly.Writers;

namespace Oligopoly;

public class Game : IWritable
{
    private readonly int _maxImprovements;
    private readonly Board _board;
    private readonly List<Player> _players = new List<Player>();
    private readonly List<Player> _observers = new List<Player>();

    private int _seed;
    private Random _random = Random.Shared;

    public Game(int maxImprovements, IReadOnlyList<Square> squares, IReadOnlyCollection<Group> groups)
    {
        if (maxImprovements < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxImprovements));
        }

        _maxImprovements = maxImprovements;
        _board = new Board(game: this, squares, groups);
    }

    public int Seed
    {
        get
        {
            return _seed;
        }
        set
        {
            if (value is 0)
            {
                _random = Random.Shared;
            }
            else
            {
                _random = new Random(value);
            }

            _seed = value;
        }
    }

    public int Players
    {
        get
        {
            return _players.Count;
        }
    }

    public event EventHandler? Started;
    public event EventHandler? TurnStarted; // Player
    public event EventHandler? TurnEnded;   // Player
    public event EventHandler? Moved; // Player
    public event EventHandler? Charging; // Player, Amount
    public event EventHandler? Charged;  // Player, Amount
    public event EventHandler? Paid; // Player, Amount
    public event EventHandler? JailEscapeAcquired;
    public event EventHandler? AuctionSucceded; // Property, Player (winner), Amount (bid)
    public event EventHandler? AuctionFailed; // Property
    public event EventHandler? DealResponded; // Outcome
    public event EventHandler? DealClosed; // Deal
    public event EventHandler? Bankrupted; // Player
    public event EventHandler? Ended; // Player (winner), Amount (rounds)
    public event EventHandler? Error; // Message

    public void Add(Player player)
    {
        _players.Add(player);
    }

    public void Start()
    {
        foreach (Player player in _players)
        {
            player.Agent.Start(game: this);
        }
    }

    private Roll Roll()
    {
        return new Roll(_random.Next(1, 7), _random.Next(1, 7));
    }

    /// <inheritdoc/>
    public void Write(Writer writer)
    {
        writer.WriteVersion();
        writer.Write(_seed);
        writer.Write(_maxImprovements);
        writer.Write(_board);
    }
}
