using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Agents;
using Oligopoly.Assets;
using Oligopoly.EventArgs;

namespace Oligopoly.Tests;

internal sealed class TestAgent : IAgent
{
    private readonly Queue<int> _mortgages = new Queue<int>();
    private readonly Queue<int> _unmortgages = new Queue<int>();
    private readonly Queue<int> _improvements = new Queue<int>();
    private readonly Queue<int> _unimprovements = new Queue<int>();
    private readonly Queue<bool> _offers = new Queue<bool>();
    private readonly Queue<int> _bids = new Queue<int>();
    private readonly Queue<Offer?> _proposals = new Queue<Offer?>();
    private readonly Queue<bool> _responses = new Queue<bool>();
    private readonly Queue<UnpoliceStrategy> _UnpoliceStrategies = new Queue<UnpoliceStrategy>();
    private readonly Queue<Warning> _warnings = new Queue<Warning>();

#nullable disable
    private Board _board;
#nullable enable

    private TestAgent() { }

    /// <inheritdoc/>
    public void OnTaxing(Game game, Player player, int amount) { }

    /// <inheritdoc/>
    public void OnTaxed(Game game, Player player, int amount) { }

    /// <inheritdoc/>
    public void OnUntaxed(Game game, Player player, int amount) { }

    public TestAgent ThenMortgage(params int[] values)
    {
        foreach (int value in values)
        {
            _mortgages.Enqueue(value);
        }

        _mortgages.Enqueue(0);

        return this;
    }

    public TestAgent ThenUnmortgage(params int[] values)
    {
        foreach (int value in values)
        {
            _unmortgages.Enqueue(value);
        }

        _unmortgages.Enqueue(0);

        return this;
    }

    public TestAgent ThenImprove(params int[] values)
    {
        foreach (int value in values)
        {
            _improvements.Enqueue(value);
        }

        _improvements.Enqueue(0);

        return this;
    }

    public TestAgent ThenUnimprove(params int[] values)
    {
        foreach (int value in values)
        {
            _unimprovements.Enqueue(value);
        }

        _unimprovements.Enqueue(0);

        return this;
    }

    public TestAgent ThenOffer(params bool[] values)
    {
        foreach (bool value in values)
        {
            _offers.Enqueue(value);
        }

        return this;
    }

    public TestAgent ThenBid(params int[] values)
    {
        foreach (int value in values)
        {
            _bids.Enqueue(value);
        }

        _bids.Enqueue(0);

        return this;
    }

    public TestAgent ThenPropose(params Offer?[] values)
    {
        foreach (Offer? value in values)
        {
            _proposals.Enqueue(value);
        }

        _proposals.Enqueue(null);

        return this;
    }

    public TestAgent ThenRespond(bool value)
    {
        _responses.Enqueue(value);
       
        return this;
    }

    public TestAgent ThenUnpolice(UnpoliceStrategy value)
    {
        _UnpoliceStrategies.Enqueue(value);

        return this;
    }

    public TestAgent ThenExpect(Warning value)
    {
        _warnings.Enqueue(value);

        return this;
    }

    /// <inheritdoc/>
    public void Connect(Controller controller)
    {
        _board = controller.Board;
        controller.Ended += OnControllerEnded;
    }

    private void OnControllerEnded(object? sender, PlayerEventArgs e)
    {
        Assert.AreEqual(0, _warnings.Count);
    }

    /// <inheritdoc/>
    public int Mortgage(Game game, Player player)
    {
        _mortgages.TryDequeue(out int id);

        Console.WriteLine("<< Mortgage [{0}]", id);

        return id;
    }

    /// <inheritdoc/>
    public int Unmortgage(Game game, Player player)
    {
        _unmortgages.TryDequeue(out int id);

        Console.WriteLine("<< Unmortgage [{0}]", id);

        return id;
    }

    /// <inheritdoc/>
    public int Improve(Game game, Player player)
    {
        _improvements.TryDequeue(out int id);

        Console.WriteLine("<< Improve [{0}]", id);

        return id;
    }

    /// <inheritdoc/>
    public int Unimprove(Game game, Player player)
    {
        _unimprovements.TryDequeue(out int id);

        Console.WriteLine("<< Unimprove [{0}]", id);

        return id;
    }

    /// <inheritdoc/>
    public bool Offer(Game game, Player player, IAsset asset)
    {
        _offers.TryDequeue(out bool result);

        Console.WriteLine("<< Offer [{0}] for [{1}]", result, asset.GetDescription(_board));

        return result;
    }

    /// <inheritdoc/>
    public int Bid(Game game, Player player, Offer offer)
    {
        _bids.TryDequeue(out int id);

        Console.WriteLine("<< Bid [{0}] on [{1}] at [{2}]", id, offer.Asset.GetDescription(_board), offer.Amount);

        return id;
    }

    /// <inheritdoc/>
    public Offer? Propose(Game game, Player player)
    {
        _proposals.TryDequeue(out Offer? result);

        if (result is null)
        {
            Console.WriteLine("<< Propose [null]");
        }
        else
        {
            Console.WriteLine("<< Propose [{0}] for [{1}] to [{2}]", result.Asset.GetDescription(_board), result.Amount, result.Player);
        }

        return result;
    }

    /// <inheritdoc/>
    public bool Respond(Game game, Player player)
    {
        _responses.TryDequeue(out bool result);

        Console.WriteLine("<< Respond [{0}]", result);

        return result;
    }

    /// <inheritdoc/>
    public UnpoliceStrategy Unpolice(Game game, Player player)
    {
        _UnpoliceStrategies.TryDequeue(out UnpoliceStrategy result);

        Console.WriteLine("<< Unpolice [{0}]", result);

        return result;
    }

    /// <inheritdoc/>
    public void Warn(Game game, Player player, Warning warning)
    {
        _warnings.TryDequeue(out Warning expectedWarning);

        Assert.AreEqual(expectedWarning, warning);
    }

    public static TestAgent Create()
    {
        return new TestAgent();
    }
}
