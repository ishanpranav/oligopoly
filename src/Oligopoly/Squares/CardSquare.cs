using System;
using System.Text.Json.Serialization;
using MessagePack;
using Oligopoly.Cards;

namespace Oligopoly.Squares;

[MessagePackObject]
public class CardSquare : ISquare
{
    public CardSquare(int deckId)
    {
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

    /// <inheritdoc/>
    public void Advance(Player player, Controller controller)
    {
        ICard card = controller.Game.Draw(controller.Board.Decks[DeckId - 1]);

        Console.WriteLine("Drew {0}", card);

        card.Draw(player, controller);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
