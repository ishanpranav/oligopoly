using MessagePack;

namespace Oligopoly.Squares;

[MessagePackObject]
public class TaxSquare : ISquare
{
    public TaxSquare(string name, int amount)
    {
        Name = name;
        Amount = amount;
    }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    [Key(1)]
    public int Amount { get; }

    /// <inheritdoc/>
    public void Advance(Player player, Controller controller)
    {
        controller.Tax(player, Amount);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
