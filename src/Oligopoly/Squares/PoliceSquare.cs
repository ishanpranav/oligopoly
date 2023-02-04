﻿using System;
using MessagePack;

namespace Oligopoly.Squares;

[MessagePackObject]
public class PoliceSquare : ISquare
{
    public PoliceSquare(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }


    /// <inheritdoc/>
    public void Land(Player player) { }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
