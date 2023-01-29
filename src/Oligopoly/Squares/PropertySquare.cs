using System;
using System.Collections.Generic;
using Oligopoly.Writers;

namespace Oligopoly.Squares;

public abstract class PropertySquare : Square
{
    protected PropertySquare(string name, IReadOnlyList<int> rents, Group group)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(rents);
        ArgumentNullException.ThrowIfNull(group);

        group.Add(this);

        Name = name;
        Rents = rents;
        Group = group;
    }

    /// <inheritdoc/>
    public override string Name { get; }

    /// <inheritdoc/>
    public override SquareType Type
    {
        get
        {
            return SquareType.Street;
        }
    }

    public abstract int Cost { get; }
    public IReadOnlyList<int> Rents { get; }
    protected Group Group { get; }

    /// <inheritdoc/>
    public override void Write(Writer writer)
    {
        base.Write(writer);

        writer.Write(Name);
    }
}
