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
        int groupLength = ReadInteger();
        Group[] groups = new Group[groupLength];
        
        for (int id = 0; id < groupLength; id++)
        {
            groups[id] = new Group(id, ReadString(), ReadInteger());
        }

        int squareLength = ReadInteger();
        List<int> utilityRents = new List<int>();
        List<int> railroadRents = new List<int>();
        Square[] squares = new Square[squareLength];

        for (int i = 0; i < squareLength; i++)
        {
            switch ((SquareType)_reader.ReadByte())
            {
                case SquareType.None:
                    squares[i] = new EmptySquare(ReadString());

                    break;

                case SquareType.Jail:
                    squares[i] = new JailSquare(ReadString());

                    break;

                case SquareType.Police:
                    squares[i] = new PoliceSquare(ReadString());

                    break;

                case SquareType.Card:
                    squares[i] = new CardSquare(ReadInteger());

                    break;

                case SquareType.Utility:
                    squares[i] = new UtilitySquare(ReadString(), utilityRents, groups[0]);

                    utilityRents.Add(0);
                    break;

                case SquareType.Railroad:
                    squares[i] = new RailroadSquare(ReadString(), railroadRents, groups[1]);

                    railroadRents.Add(0);
                    break;

                case SquareType.Street:
                    string name = ReadString();
                    int[] rents = new int[settings.MaxImprovements + 1];

                    for (int j = 0; j < rents.Length; j++)
                    {
                        rents[j] = ReadInteger();
                    }

                    squares[i] = new StreetSquare(name, rents, groups[ReadInteger()], ReadInteger());

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
