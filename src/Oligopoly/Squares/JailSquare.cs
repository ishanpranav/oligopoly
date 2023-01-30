﻿using System;
using MessagePack;

namespace Oligopoly.Squares;

[MessagePackObject]
public class JailSquare : ISquare
{
    public JailSquare(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }
}
