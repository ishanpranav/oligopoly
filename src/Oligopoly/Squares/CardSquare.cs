using System;
using System.Text.Json.Serialization;
using MessagePack;

namespace Oligopoly.Squares;

[MessagePackObject]
public class CardSquare : ISquare
{
    public CardSquare(int deckId)
    {
        if (deckId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(deckId));
        }

        DeckId = deckId;
    }

    [JsonPropertyName("deck")]
    [Key(0)]
    public int DeckId { get; }

    [IgnoreMember]
    [JsonIgnore]
    public Deck? Deck { get; set; }

    /// <inheritdoc/>
    [IgnoreMember]
    public string Name
    {
        get
        {
            if (Deck is null)
            {
                return DeckId.ToString();
            }

            return Deck.ToString();
        }
    }
}
