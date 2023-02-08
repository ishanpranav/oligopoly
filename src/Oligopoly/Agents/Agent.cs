using System;
using Oligopoly.EventArgs;

namespace Oligopoly.Agents;

public class Agent : IAgent
{
    /// <inheritdoc/>
    public virtual void Connect(GameController controller)
    {
        Console.WriteLine(">> Connect");

        controller.Started += OnControllerStarted;
        controller.TurnStarted += OnControllerTurnStarted;
        controller.Advanced += OnControllerAdvanced;
        controller.AuctionFailed += OnControllerAuctionFailed;
        controller.AuctionSucceeded += OnControllerAuctionSucceeded;
    }

    private void OnControllerStarted(object? sender, GameEventArgs e)
    {
        Console.WriteLine("== Started");
    }

    private void OnControllerTurnStarted(object? sender, GameEventArgs e)
    {
        Console.WriteLine("== Turn Started");
    }

    private void OnControllerAuctionFailed(object? sender, AuctionEventArgs e)
    {
        Console.WriteLine("== Auction Failed {0}", e.Asset);
    }

    private void OnControllerAuctionSucceeded(object? sender, AuctionEventArgs e)
    {
        Console.WriteLine("== Auction Succeeded {0}, {1}", e.Asset, e.Bid);
    }

    private void OnControllerAdvanced(object? sender, PlayerEventArgs e)
    {
        Console.WriteLine("== Advanced {0}", e.Player);
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
    public virtual int Bid(Game game, Player player, IAsset asset, int bid)
    {
        Console.WriteLine("<< Bid [0] against {1} for {0}", asset, bid);

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
