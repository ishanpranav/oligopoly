using MessagePack;

namespace Oligopoly.Assets;

[MessagePackObject]
public readonly struct CardId : IAsset
{
    [SerializationConstructor]
    public CardId(int id, int deckId)
    {
        Id = id;
        DeckId = deckId;
    }

    [Key(0)]
    public int Id { get; }

    [Key(1)]
    public int DeckId { get; }

    /// <inheritdoc/>
    public bool IsImproved
    {
        get
        {
            return true;
        }
    }

    /// <inheritdoc/>
    public int GetPlayerId(Game game)
    {
        foreach (Player player in game.Players)
        {
            if (player.CardIds.Contains(this))
            {
                return player.Id;
            }
        }

        return 0;
    }

    /// <inheritdoc/>
    public string GetDescription(Board board)
    {
        return board.Decks[DeckId - 1].Cards[Id - 1].Name;
    }

    /// <inheritdoc/>
    public void Transfer(GameController controller, Player sender, Player recipient)
    {
        while (sender.CardIds.TryDequeue(out CardId cardId))
        {
            recipient.CardIds.Enqueue(cardId);
        }
    }

    /// <inheritdoc/>
    public void Discard(GameController controller, Player player)
    {
        while (player.CardIds.TryDequeue(out CardId cardId))
        {
            controller.Game.Discard(cardId);
        }
    }

    /// <inheritdoc/>
    public int Appraise(Board board, Game game)
    {
        return 50;
    }
}
