using System.Text.Json.Serialization;
using MessagePack;
using Oligopoly.Squares;

namespace Oligopoly;

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
    public int Appraise(Board board, Game game)
    {
        return ((IAsset)board.Squares[SquareId - 1]).Appraise(board, game);
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
