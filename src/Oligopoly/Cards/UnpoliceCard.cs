using System.Text.Json.Serialization;
using MessagePack;
using Oligopoly.Assets;

namespace Oligopoly.Cards;

[MessagePackObject]
public class UnpoliceCard : IAppraisable, ICard
{
    public UnpoliceCard(string name)
    {
        Name = name;
    }

    /// <inheritdoc/>
    [IgnoreMember]
    [JsonIgnore]
    public CardId Id { get; set; }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    /// <inheritdoc/>
    public int Appraise(Board board, Game game)
    {
        return Id.Appraise(board, game);
    }

    /// <inheritdoc/>
    public void Draw(Player player, Controller controller)
    {
        player.CardIds.Enqueue(Id);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
