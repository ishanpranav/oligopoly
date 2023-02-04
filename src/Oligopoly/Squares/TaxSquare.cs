using System;
using MessagePack;

namespace Oligopoly.Squares;

[MessagePackObject]
public class TaxSquare : ISquare
{
    public TaxSquare(string name, int amount)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }

        Name = name;
        Amount = amount;
    }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    [Key(1)]
    public int Amount { get; }

    /// <inheritdoc/>
    public void Land(Player player) { }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
