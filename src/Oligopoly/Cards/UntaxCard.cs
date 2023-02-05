﻿using MessagePack;
using System;

namespace Oligopoly.Cards;

[MessagePackObject]
public class UntaxCard : ICard
{
    public UntaxCard(string name, int amount)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (amount <= 0)
        {
            throw new ArgumentNullException(nameof(amount));
        }

        Name = name;
        Amount = amount;
    }

    /// <inheritdoc/>
    [IgnoreMember]
    public CardId Id { get; set; }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    [Key(1)]
    public int Amount { get; }

    /// <inheritdoc/>
    public void Draw(Player player, GameController controller)
    {
        controller.Untax(player, Amount);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
