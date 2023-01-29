using System;
using System.Collections.Generic;
using Oligopoly.Writers;

namespace Oligopoly.Squares;

public class RailroadSquare : Square
{
    public RailroadSquare(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }

    /// <inheritdoc/>
    public override string Name { get; }

    public Group? Group { get; set; }

    /// <inheritdoc/>
    public override SquareType Type
    {
        get
        {
            return SquareType.Railroad;
        }
    }

    /// <inheritdoc/>
    public override void Write(Writer writer)
    {
        base.Write(writer);

        writer.Write(Name);
    }
}
