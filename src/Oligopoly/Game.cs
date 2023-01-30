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

    public void Add(Player player)
    {
        _players.Add(player);
    }

    public void Start()
    {

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
