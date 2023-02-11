using System.Collections.Generic;
using MessagePack;
using Oligopoly.Assets;

namespace Oligopoly.Squares;

[MessagePackObject]
public class RailroadSquare : PropertySquare
{
    public RailroadSquare(string name) : base(name) { }

    /// <inheritdoc/>
    public override int GetRent(int squareId, Player owner, GameController controller)
    {
        int count;

        if (controller.Flying)
        {
            count = controller.Board.RailroadFares.Count - 1;
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

                if (controller.Board.Squares[indexedDeed.Key] is not RailroadSquare)
                {
                    continue;
                }

                count++;
            }
        }

        return controller.Board.RailroadFares[count];
    }

    /// <inheritdoc/>
    public override int Appraise(Board board, Game game)
    {
        return board.RailroadCost;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }
}
