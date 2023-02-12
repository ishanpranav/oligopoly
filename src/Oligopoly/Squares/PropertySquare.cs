using MessagePack;
using Oligopoly.Assets;

namespace Oligopoly.Squares;

public abstract class PropertySquare : IAppraisable, ISquare
{
    protected PropertySquare(string name)
    {
        Name = name;
    }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    public abstract int GetRent(int squareId, Player owner, Controller controller);

    /// <inheritdoc/>
    public abstract int Appraise(Board board, Game game);

    /// <inheritdoc/>
    public void Advance(Player player, Controller controller)
    {
        Deed deed = controller.Game.Deeds[player.SquareId - 1];

        if (deed.PlayerId == player.Id || deed.Mortgaged)
        {
            return;
        }

        if (deed.PlayerId is 0)
        {
            controller.Offer(player, deed);

            return;
        }

        foreach (Player owner in controller.Game.Players)
        {
            if (deed.PlayerId == owner.Id)
            {
                controller.Gift(player, owner, GetRent(player.SquareId, owner, controller));

                break;
            }
        }
    }
}
