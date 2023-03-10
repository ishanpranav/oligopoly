using MessagePack;

namespace Oligopoly.Squares;

[MessagePackObject]
public class EmptySquare : ISquare
{
    public EmptySquare(string name)
    {
        Name = name;
    }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    /// <inheritdoc/>
    public void Advance(Player player, Controller controller) { }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
