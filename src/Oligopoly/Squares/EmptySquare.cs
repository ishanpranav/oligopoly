using System;
using Oligopoly.Writers;

namespace Oligopoly.Squares;

public class EmptySquare : Square
{
    public EmptySquare(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }

    /// <inheritdoc/>
    public override string Name { get; }

    /// <inheritdoc/>
    public override SquareType Type
    {
        get
        {
            return SquareType.None;
        }
    }

    /// <inheritdoc/>
    public override void Write(Writer writer)
    {
        base.Write(writer);

        writer.Write(Name);
    }
}
