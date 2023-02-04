using System;
using Oligopoly.Agents;
using Oligopoly.Cards;
using Oligopoly.EventArgs;

namespace Oligopoly;

public class GameController
{
    private readonly Board _board;
    private readonly Game _game;

    private bool _proposing;

    public GameController(Board board, Game game)
    {
        ArgumentNullException.ThrowIfNull(board);
        ArgumentNullException.ThrowIfNull(game);

        _board = board;
        _game = game;
    }

    public event EventHandler<GameEventArgs>? Started;
    public event EventHandler<GameEventArgs>? TurnStarted;

    public void Start()
    {
        Console.WriteLine("Start of game. Players:");

        foreach (Player player in _game.Players)
        {
            player.Agent.Connect(controller: this);

            Console.WriteLine("\t{0}", player.Name);
        }

        OnStarted(new GameEventArgs(_game));
    }

    public bool MoveNext()
    {
        Player current = _game.Current;

        if (current.Cash < 0)
        {
            return true;
        }

        Console.WriteLine();
        Console.WriteLine("Start of turn {0} for {1}", _game.Turn, current);
        Console.WriteLine("Cash=${0}, Net Worth=${1}", current.Cash, current.Appraise(_board));

        if (current.JailTurns > 0)
        {
            current.JailTurns++;
        }

        OnTurnStarted(new GameEventArgs(_game));
        Propose(current);
        Jailbreak(current);
        Unmortgage(current);

        _game.Turn++;

        foreach (Player player in _game.Players)
        {
            if (player.Cash >= 0)
            {
                return true;
            }
        }

        return false;
    }

    private void Propose(Player player)
    {
        if (!_proposing)
        {
            _proposing = true;

            while (player.Agent.Propose() is not null)
                ;

            _proposing = false;
        }
    }

    private void Jailbreak(Player player)
    {
        if (player.JailTurns > 0)
        {
            switch (player.Agent.Jailbreak(_game, player.Id))
            {
                case JailbreakStrategy.Bail:
                    Tax(player, amount: 50);

                    player.JailTurns = 0;

                    break;

                case JailbreakStrategy.Card:
                    if (player.TryPlay(out CardId cardId))
                    {
                        player.JailTurns = 0;

                        _game.Discard(cardId);
                    }
                    break;

                default:

                    break;
            }
        }
    }

    private void Unmortgage(Player player)
    {
        while (true)
        {
            int deedId = player.Agent.Unmortgage(_game, player);

            if (deedId is 0)
            {
                break;
            }

            if (player.DeedIds.Contains(deedId))
            {
                player.Agent.Warn(Warning.AccessDenied);

                break;
            }

            int amount = (int)((_board.MortgageLoanProportion + _board.MortgageInterestRate) * _board.Appraise(deedId));

            Tax(player, amount);

            if (player.Cash < 0)
            {
                Untax(player, amount);
            }
            else
            {
                _game.Mortgage(deedId);
            }
        }
    }

    private void Tax(Player player, int amount)
    {
        player.Agent.Tax(amount);
        Propose(player);
        // Unimprove(player);
        // Mortgage(player);

        player.Cash -= amount;
        player.Agent.Taxed(amount);

        Console.WriteLine("{0} pays £{1}", player, amount);
    }

    private void Untax(Player player, int amount)
    {
        player.Cash += amount;
        player.Agent.Untaxed(amount);

        Console.WriteLine("{0} gets £{1}", player, amount);
    }

    private Roll Roll()
    {
        return new Roll(Random.Shared.Next(1, 7), Random.Shared.Next(1, 7));
    }

    protected virtual void OnStarted(GameEventArgs e)
    {
        if (Started is not null)
        {
            Started(sender: this, e);
        }
    }

    protected virtual void OnTurnStarted(GameEventArgs e)
    {
        if (TurnStarted is not null)
        {
            TurnStarted(sender: this, e);
        }
    }
}
