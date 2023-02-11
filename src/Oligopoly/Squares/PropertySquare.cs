using System;
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

    public abstract int GetRent(int squareId, Player owner, GameController controller);

    /// <inheritdoc/>
    public abstract int Appraise(Board board, Game game);

    /// <inheritdoc/>
    public void Advance(Player player, GameController controller)
    {
        Deed deed = controller.Game.Deeds[player.SquareId - 1];

        if (deed.PlayerId == player.Id)
        {
            Console.WriteLine("{0} already owns this property", player);

            return;
        }

        if (deed.Mortgaged)
        {
            Console.WriteLine("Property is mortgaged");

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
                int rent = GetRent(player.SquareId, owner, controller);

                Console.WriteLine("{0} must pay rent of £{1} to {2}", player, rent, owner);
                controller.Gift(player, owner, rent);

                break;
            }
        }
    }
}
