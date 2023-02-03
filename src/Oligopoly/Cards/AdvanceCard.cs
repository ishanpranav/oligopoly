using System;
using System.Text.Json.Serialization;
using MessagePack;

namespace Oligopoly.Cards;

[MessagePackObject]
public class AdvanceCard : ICard
{
    public AdvanceCard(string name, int destinationId)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (destinationId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(destinationId));
        }

        Name = name;
        DestinationId = destinationId;
    }

    /// <inheritdoc/>
    [IgnoreMember]
    public CardId Id { get; set; }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    [JsonPropertyName("destination")]
    [Key(1)]
    public int DestinationId { get; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
