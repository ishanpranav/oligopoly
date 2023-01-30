﻿using System;
using MessagePack;

namespace Oligopoly.Squares;

[MessagePackObject]
public class UtilitySquare : ISquare
{
    public UtilitySquare(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name; 
    }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }
}
