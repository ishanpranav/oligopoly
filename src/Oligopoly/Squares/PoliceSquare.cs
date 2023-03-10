using MessagePack;

namespace Oligopoly.Squares;

[MessagePackObject]
public class PoliceSquare : ISquare
{
    public PoliceSquare(string name)
    {
        Name = name;
    }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    /// <inheritdoc/>
    public void Advance(Player player, Controller controller)
    {
        controller.Police(player);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
