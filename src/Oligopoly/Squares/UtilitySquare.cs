using System.Collections.Generic;
using MessagePack;
using Oligopoly.Assets;

namespace Oligopoly.Squares;

[MessagePackObject]
public class UtilitySquare : PropertySquare
{
    public UtilitySquare(string name) : base(name) { }

    /// <inheritdoc/>
    public override int GetRent(int squareId, Player owner, GameController controller)
    {
        int count;

        if (controller.Flying)
        {
            count = controller.Board.UtilityBillMultipliers.Count - 1;
        }
        else
        {
            count = -1;

            foreach (KeyValuePair<int, Deed> indexedDeed in controller.Game.Deeds)
            {
                if (indexedDeed.Value.PlayerId != owner.Id)
                {
                    continue;
                }

                if (controller.Board.Squares[indexedDeed.Key] is not UtilitySquare)
                {
                    continue;
                }

                count++;
            }
        }

        return controller.Board.UtilityBillMultipliers[count] * controller.Game.Dice.Amount;
    }

    /// <inheritdoc/>
    public override int Appraise(Board board, Game game)
    {
        return board.UtilityCost;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
