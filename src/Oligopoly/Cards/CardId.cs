using System;
using MessagePack;

namespace Oligopoly.Cards;

[MessagePackObject]
public readonly struct CardId
{
    [SerializationConstructor]
    public CardId(int id, int deckId)
    {
        if (id <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }

        if (deckId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(deckId));
        }

        Id = id;
        DeckId = deckId;
    }

    [Key(0)]
    public int Id { get; }

    [Key(1)]
    public int DeckId { get; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Id}-{DeckId}";
    }
}
