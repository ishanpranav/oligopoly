using MessagePack;

namespace Oligopoly;

[MessagePackObject]
public class Group
{
    public Group(string name, int improvementCost)
    {
        Name = name;
        ImprovementCost = improvementCost;
    }

    [Key(0)]
    public string Name { get; }

    [Key(1)]
    public int ImprovementCost { get; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
