using System;

namespace Oligopoly.Cards;

public readonly struct CardId
{
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
    }

    public int Id { get; }
    public int DeckId { get; }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"{Id}-{DeckId}";
    }
}
