using Oligopoly.Writers;

namespace Oligopoly.Squares;

public abstract class Square : IWritable
{
    private static EmptySquare? s_emptySquare;
    private static JailSquare? s_jailSquare;
    private static PoliceSquare? s_policeSquare;

    protected Square() { }

    public static Square Empty
    {
        get
        {
            if (s_emptySquare is null)
            {
                s_emptySquare = new EmptySquare();
            }

            return s_emptySquare;
        }
    }

    public static Square Jail
    {
        get
        {
            if (s_jailSquare is null)
            {
                s_jailSquare = new JailSquare();
            }

            return s_jailSquare;
        }
    }

    public static Square Police
    {
        get
        {
            if (s_policeSquare is null)
            {
                s_policeSquare = new PoliceSquare();
            }

            return s_policeSquare;
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
