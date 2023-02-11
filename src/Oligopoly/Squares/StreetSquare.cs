using System.Collections.Generic;
using System.Text.Json.Serialization;
using MessagePack;
using Oligopoly.Assets;

namespace Oligopoly.Squares;

[MessagePackObject]
public class StreetSquare : PropertySquare
{
    public StreetSquare(string name, int cost, int groupId, IReadOnlyList<int> rents) : base(name)
    {
        Cost = cost;
        GroupId = groupId;
        Rents = rents;
    }

    [Key(1)]
    public int Cost { get; }

    [JsonPropertyName("group")]
    [Key(2)]
    public int GroupId { get; }

    [IgnoreMember]
    [JsonIgnore]
    public Group? Group { get; set; }

    [Key(3)]
    public IReadOnlyList<int> Rents { get; }

    /// <inheritdoc/>
    public override int GetRent(int squareId, Player owner, GameController controller)
    {
        Deed deed = controller.Game.Deeds[squareId - 1];

        if (deed.Improvements is 0)
        {
            foreach (KeyValuePair<int, Deed> indexedDeed in controller.Game.Deeds)
            {
                if (indexedDeed.Key == squareId - 1)
                {
                    continue;
                }

                if (controller.Board.Squares[indexedDeed.Key] is not StreetSquare other)
                {
                    continue;
                }

                if (other.GroupId != GroupId)
                {
                    continue;
                }

                if (indexedDeed.Value.PlayerId != owner.Id)
                {
                    return Rents[0];
                }
            }

            return Rents[0] * controller.Board.GroupRentMultiplier;
        }

        return Rents[deed.Improvements];
    }

    /// <inheritdoc/>
    public override int Appraise(Board board, Game game)
    {
        return Cost;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
