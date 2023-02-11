using System;
using Oligopoly.Assets;
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
    public JailbreakStrategy Jailbreak(Game game, Player player)
    {
        Console.WriteLine("<< Jailbreak [NONE]");

        return JailbreakStrategy.None;
    }

    /// <inheritdoc/>
    public virtual void Warn(Game game, Player player, Warning warning)
    {
        throw new InvalidOperationException(warning.ToString());
    }
}
