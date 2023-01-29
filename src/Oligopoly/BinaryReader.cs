using System;
using System.Collections.Generic;
using System.IO;
using Oligopoly.Squares;

namespace Oligopoly;

public class BinaryReader : IDisposable
{
    private readonly System.IO.BinaryReader _reader;

    private bool _disposed;

    public BinaryReader(Stream input)
    {
        ArgumentNullException.ThrowIfNull(input);

        _reader = new System.IO.BinaryReader(input);
    }

    public Board ReadBoard()
    {
        if (_reader.ReadUInt16() is not 12004)
        {
            throw new FormatException();
        }

        int railroadCost = _reader.ReadInt32();
        int squareLength = _reader.ReadInt32();
        List<int> railroadRents = new List<int>();
        Square[] squares = new Square[squareLength];

        for (int i = 0; i < squareLength; i++)
        {
            switch ((SquareType)_reader.ReadByte())
            {
                case SquareType.Start:
                    squares[i] = Square.Start;

                    break;

                case SquareType.Street:
                    string name = _reader.ReadString();
                    int cost = _reader.ReadInt32();
                    int rentLength = _reader.ReadInt32();
                    int[] rents = new int[rentLength];

                    for (int j = 0; j < rentLength; j++)
                    {
                        rents[j] = _reader.ReadInt32();
                    }

                    squares[i] = new StreetSquare(name, cost, rents);

                    break;

                case SquareType.Card:
                    squares[i] = new CardSquare();

                    break;

                case SquareType.Tax:
                    squares[i] = new TaxSquare(_reader.ReadString(), _reader.ReadInt32());

                    break;

                case SquareType.Railroad:
                    squares[i] = new RailroadSquare(_reader.ReadString());

                    railroadRents.Add(0);
                    break;

                case SquareType.Jail:
                    squares[i] = Square.Jail;

                    break;

                default:
                    throw new FormatException();
            }
        }

        for (int i = 0; i < railroadRents.Count; i++)
        {
            railroadRents[i] = _reader.ReadInt32();
        }

        int groupLength = _reader.ReadInt32();

        Group[] groups = Array.Empty<Group>();

        return new Board(railroadCost, squares, railroadRents, groups);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _reader.Dispose();
            }

            _disposed = true;
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
