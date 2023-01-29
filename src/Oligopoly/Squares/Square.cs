using Oligopoly.Writers;

namespace Oligopoly.Squares;

public abstract class Square : IWritable
{
    public SquareType Type { get; }

    /// <inheritdoc/>
    public void Write(Writer writer)
    {
        writer.Write((byte)Type);
    }
}
