﻿using System;
using MessagePack;

namespace Oligopoly.Cards;

[MessagePackObject]
public class JailEscapeCard : ICard
{
    public JailEscapeCard(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }

    /// <inheritdoc/>
    [IgnoreMember]
    public CardId Id { get; set; }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
