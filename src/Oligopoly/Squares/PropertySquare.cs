using System;
using MessagePack;

namespace Oligopoly.Squares;

public abstract class PropertySquare : IAsset, ISquare
{
    protected PropertySquare(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }

    /// <inheritdoc/>
    [Key(0)]
    public string Name { get; }

    public abstract int GetRent(Board board, int roll);

    /// <inheritdoc/>
    public abstract int Appraise(Board board, Game game);

    /// <inheritdoc/>
    public void Land(Player player, GameController controller)
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
        }
        else
        {
            int rent = GetRent(controller.Board, controller.Roll);
            Player owner = controller.Game.Players[deed.PlayerId - 1];

            Console.WriteLine("{0} must pay rent of £{1} to {2}", player, rent, owner);
            controller.Transfer(player, owner, rent);
        }
    }
}
