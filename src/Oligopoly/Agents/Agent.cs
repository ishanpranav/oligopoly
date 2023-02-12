using System;
using Oligopoly.Assets;
using Oligopoly.EventArgs;

namespace Oligopoly.Agents;

public class Agent : IAgent
{
    /// <inheritdoc/>
    public virtual void Connect(Controller controller)
    {
        Console.WriteLine(">> Connect");

        controller.TurnStarted += OnControllerTurnStarted;
        controller.TurnEnded += OnControllerTurnEnded;
        controller.Advanced += OnControllerAdvanced;
        controller.AuctionFailed += OnControllerAuctionFailed;
        controller.AuctionSucceeded += OnControllerAuctionSucceeded;
        controller.Ended += OnControllerEnded;
    }

    private void OnControllerTurnStarted(object? sender, PlayerEventArgs e)
    {
        Console.WriteLine("== Turn Started");
    }

    private void OnControllerTurnEnded(object? sender, PlayerEventArgs e)
    {
        Console.WriteLine("== Turn Ended");
    }

    private void OnControllerAdvanced(object? sender, PlayerEventArgs e)
    {
        Console.WriteLine("== Advanced {0}", e.Player);
    }

    private void OnControllerAuctionFailed(object? sender, AuctionEventArgs e)
    {
        Console.WriteLine("== Auction Failed {0}", e.Asset);
    }

    private void OnControllerAuctionSucceeded(object? sender, AuctionEventArgs e)
    {
        Console.WriteLine("== Auction Succeeded {0}, {1}", e.Asset, e.Bid);
    }

    private void OnControllerEnded(object? sender, PlayerEventArgs e)
    {
        Console.WriteLine("== Ended {0}", e.Player);
    }

    /// <inheritdoc/>
    public void OnTaxing(Game game, Player player, int amount)
    {
        Console.WriteLine(">> Tax {0}", amount);
    }

    /// <inheritdoc/>
    public void OnTaxed(Game game, Player player, int amount)
    {
        Console.WriteLine(">> Taxed {0}", amount);
    }

    /// <inheritdoc/>
    public void OnUntaxed(Game game, Player player, int amount)
    {
        Console.WriteLine(">> Untaxed {0}", amount);
    }

    /// <inheritdoc/>
    public virtual int Mortgage(Game game, Player player)
    {
        return 0;
    }

    /// <inheritdoc/>
    public virtual int Unmortgage(Game game, Player player)
    {
        Console.WriteLine("<< Unmortgage [0]");

        return 0;
    }

    /// <inheritdoc/>
    public virtual int Improve(Game game, Player player)
    {
        return 0;
    }

    /// <inheritdoc/>
    public virtual int Unimprove(Game game, Player player)
    {
        return 0;
    }

    /// <inheritdoc/>
    public virtual bool Offer(Game game, Player player, IAsset asset)
    {
        return false;
    }

    /// <inheritdoc/>
    public virtual int Bid(Game game, Player player, Offer offer)
    {
        return 0;
    }

    /// <inheritdoc/>
    public virtual Offer? Propose(Game game, Player player)
    {
        return null;
    }

    /// <inheritdoc/>
    public virtual bool Respond(Game game, Player player)
    {
        return false;
    }

    /// <inheritdoc/>
    public virtual UnpoliceStrategy Unpolice(Game game, Player player)
    {
        return UnpoliceStrategy.None;
    }

    /// <inheritdoc/>
    public virtual void Warn(Game game, Player player, Warning warning)
    {
        throw new InvalidOperationException(warning.ToString());
    }
}
