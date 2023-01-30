using System;
using System.Collections.Generic;
using Oligopoly.Squares;
using Oligopoly.Writers;

namespace Oligopoly;

public class Group : IWritable
{
    private readonly string _name;
    private readonly List<PropertySquare> _properties = new List<PropertySquare>();

    public Group(int id, string name, int improvementCost)
    {
        if (id < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }

        ArgumentNullException.ThrowIfNull(name);

        if (improvementCost <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(improvementCost));
        }

        Id = id;
        _name = name;
        ImprovementCost = improvementCost;
    }

    public int Id { get; }
    public int ImprovementCost { get; }

    public void Add(PropertySquare property)
    {
        _properties.Add(property);
    }

    /// <inheritdoc/>
    public void Write(Writer writer)
    {
        writer.Write(_name);
        writer.Write(ImprovementCost);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return _name;
    }
}
