using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Oligopoly.Squares;

namespace Oligopoly;

public class BinaryReader : IDisposable
{
    private readonly System.IO.BinaryReader _reader;

    private bool _disposed;

    public BinaryReader(Stream input)
    {
        ArgumentNullException.ThrowIfNull(input);

        _reader = new System.IO.BinaryReader(input, Encoding.ASCII);
    }

    public int ReadInteger()
    {
        return _reader.ReadUInt16();
    }

    public string ReadString()
    {
        return _reader.ReadString();
    }

    public BoardSettings ReadBoardSettings()
    {
        if (_reader.ReadUInt16() is not 12004)
        {
            throw new FormatException();
        }

        return new BoardSettings(ReadInteger());
    }

    public Board ReadBoard()
    {
        BoardSettings settings = ReadBoardSettings();
        int utilityCost = ReadInteger();
        int railroadCost = ReadInteger();
        int squareLength = ReadInteger();
        List<int> utilityRents = new List<int>();
        List<int> railroadRents = new List<int>();
        Square[] squares = new Square[squareLength];

        for (int i = 0; i < squareLength; i++)
        {
            switch ((SquareType)_reader.ReadByte())
            {
                case SquareType.None:
                    squares[i] = Square.Empty;

                    break;

                case SquareType.Jail:
                    squares[i] = Square.Jail;

                    break;

                case SquareType.Police:
                    squares[i] = Square.Police;

                    break;

                case SquareType.Card:
                    squares[i] = new CardSquare(ReadInteger());

                    break;

                case SquareType.Utility:
                    squares[i] = new UtilitySquare(ReadString(), utilityCost, utilityRents);

                    utilityRents.Add(0);
                    break;

                case SquareType.Railroad:
                    squares[i] = new RailroadSquare(ReadString(), railroadCost, railroadRents);

                    railroadRents.Add(0);
                    break;

                case SquareType.Street:
                    string name = ReadString();
                    int cost = ReadInteger();
                    int[] rents = new int[settings.MaxImprovements + 1];

                    for (int j = 0; j < rents.Length; j++)
                    {
                        rents[j] = ReadInteger();
                    }

                    squares[i] = new StreetSquare(name, cost, rents);

                    break;

                case SquareType.Tax:
                    squares[i] = new TaxSquare(ReadString(), ReadInteger());

                    break;

                default:
                    throw new FormatException();
            }
        }

        for (int i = 0; i < utilityRents.Count; i++)
        {
            utilityRents[i] = ReadInteger();
        }

        for (int i = 0; i < railroadRents.Count; i++)
        {
            railroadRents[i] = ReadInteger();
        }

        int groupLength = ReadInteger();

        Group[] groups = Array.Empty<Group>();

        return new Board(settings, squares, groups);
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
