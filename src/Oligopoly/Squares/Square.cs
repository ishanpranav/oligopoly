using Oligopoly.Writers;

namespace Oligopoly.Squares;

public abstract class Square : IWritable
{
    protected Square() { }

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
