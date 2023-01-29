using Oligopoly.Writers;

namespace Oligopoly.Squares;

public abstract class Square : IWritable
{
    private static StartSquare? s_startSquare;

    protected Square() { }

    public static Square Start
    {
        get
        {
            if (s_startSquare is null)
            {
                s_startSquare = new StartSquare();
            }

            return s_startSquare;
        }
    }

    public virtual string Name
    {
        get
        {
            return Type.ToString();
        }
    }

    public abstract SquareType Type { get; }

    /// <inheritdoc/>
    public virtual void Write(Writer writer)
    {
        writer.Write(Type);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
