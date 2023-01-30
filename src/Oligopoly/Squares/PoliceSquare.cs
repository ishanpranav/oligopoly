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
}
