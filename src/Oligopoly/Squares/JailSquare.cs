using MessagePack;

namespace Oligopoly.Squares;

[MessagePackObject]
public class JailSquare : ISquare
{
    public JailSquare(string name)
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
