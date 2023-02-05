using System.Text.Json.Serialization;
using MessagePack;

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

    public int Appraise(Board board, Game game)
    {
        return ((IAsset)board.Squares[SquareId - 1]).Appraise(board, game);
    }
}
