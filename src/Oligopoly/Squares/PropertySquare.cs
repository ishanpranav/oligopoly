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
    public void Land(GameController controller)
    {
        Deed deed = controller.Game.Deeds[controller.Game.Current.SquareId - 1];

        if (deed.PlayerId == controller.Game.Current.Id)
        {
            Console.WriteLine("{0} already owns this property", controller.Game.Current);

            return;
        }

        if (deed.Mortgaged)
        {
            Console.WriteLine("Property is mortgaged");

            return;
        }

        if (deed.PlayerId is 0)
        {
            controller.Offer(controller.Game.Current, deed);
        }
        else
        {
            int rent = GetRent(controller.Board, controller.Roll);
            Player owner = controller.Game.Players[deed.PlayerId - 1];

            Console.WriteLine("{0} must pay rent of £{1} to {2}", controller.Game.Current, rent, owner);
            controller.Transfer(controller.Game.Current, owner, rent);
        }
    }
}
