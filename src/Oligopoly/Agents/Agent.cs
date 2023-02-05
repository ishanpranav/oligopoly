using System;

namespace Oligopoly.Agents;

internal sealed class Agent : IAgent
{
    /// <inheritdoc/>
    public void Connect(GameController controller)
    {
        Console.WriteLine(">> Connect");
    }

    /// <inheritdoc/>
    public void Tax(Game game, Player player, int amount)
    {
        Console.WriteLine(">> Tax {0}", amount);
    }

    /// <inheritdoc/>
    public void Taxed(Game game, Player player, int amount)
    {
        Console.WriteLine(">> Taxed {0}", amount);
    }

    /// <inheritdoc/>
    public void Untaxed(Game game, Player player, int amount)
    {
        Console.WriteLine(">> Untaxed {0}", amount);
    }

    /// <inheritdoc/>
    public int Mortgage(Game game, Player player)
    {
        Console.WriteLine("<< Mortgage [0]");

        return 0;
    }

    /// <inheritdoc/>
    public int Unmortgage(Game game, Player player)
    {
        Console.WriteLine("<< Unmortgage [0]");

        return 0;
    }

    /// <inheritdoc/>
    public int Improve(Game game, Player player)
    {
        Console.WriteLine("<< Improve [0]");

        return 0;
    }

    /// <inheritdoc/>
    public int Unimprove(Game game, Player player)
    {
        Console.WriteLine("<< Unimprove [0]");

        return 0;
    }

    /// <inheritdoc/>
    public bool Offer(Game game, Player player, IAsset asset)
    {
        Console.WriteLine("<< Offer [FALSE] for {0}", asset);

        return false;
    }

    /// <inheritdoc/>
    public int Bid(Game game, Player player, IAsset asset)
    {
        Console.WriteLine("<< Bid [0] for {0}", asset);

        return 0;
    }

    /// <inheritdoc/>
    public DealProposal? Propose(Game game, Player player)
    {
        Console.WriteLine("<< Propose [0]");

        return null;
    }

    /// <inheritdoc/>
    public JailbreakStrategy Jailbreak(Game game, Player player)
    {
        Console.WriteLine("<< Jailbreak [NONE]");

        return JailbreakStrategy.None;
    }

    /// <inheritdoc/>
    public void Warn(Game game, Player player, Warning warning)
    {
        throw new InvalidOperationException(warning.ToString());
    }
}
