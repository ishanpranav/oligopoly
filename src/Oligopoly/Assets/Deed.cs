using System.Text.Json.Serialization;
using MessagePack;
using Oligopoly.Squares;

namespace Oligopoly.Assets;

[MessagePackObject]
public class Deed : IAsset
{
    [IgnoreMember]
    [JsonIgnore]
    public int SquareId { get; set; }

    [Key(0)]
    public int PlayerId { get; set; }

    [Key(1)]
    public bool Mortgaged { get; set; }

    [Key(2)]
    public int Improvements { get; set; }

    /// <inheritdoc/>
    public int GetPlayerId(Game game)
    {
        return PlayerId;
    }

    /// <inheritdoc/>
    public string GetDescription(Board board)
    {
        return board.Squares[SquareId - 1].Name;
    }

    /// <inheritdoc/>
    public void Transfer(GameController controller, Player sender, Player recipient)
    {
        if (Improvements > 0)
        {
            return;
        }

        if (Mortgaged)
        {
            controller.Tax(recipient, (int)(controller.Board.MortgageInterestRate * Appraise(controller.Board, controller.Game)));

            PlayerId = recipient.Id;

            controller.Unmortgage(recipient);
        }
        else
        {
            PlayerId = recipient.Id;
        }
    }

    /// <inheritdoc/>
    public void Discard(GameController controller, Player player)
    {
        PlayerId = 0;
        Mortgaged = false;

        while (Improvements > 0)
        {
            Unimprove(controller.Game, (StreetSquare)controller.Board.Squares[SquareId - 1]);
        }
    }

    /// <inheritdoc/>
    public int Appraise(Board board, Game game)
    {
        return ((IAppraisable)board.Squares[SquareId - 1]).Appraise(board, game);
    }

    public void Improve(Game game, StreetSquare square)
    {
        Improvements++;

        if (Improvements == square.Rents.Count - 1)
        {
            game.Hotels--;
            game.Houses += square.Rents.Count - 2;
        }
        else
        {
            game.Houses--;
        }
    }

    public void Unimprove(Game game, StreetSquare square)
    {
        if (Improvements == square.Rents.Count - 1)
        {
            game.Hotels++;
            game.Houses -= square.Rents.Count - 2;
        }
        else
        {
            game.Houses++;
        }

        Improvements--;
    }
}
