using System;
using Oligopoly.Agents;
using Oligopoly.Cards;
using Oligopoly.EventArgs;

namespace Oligopoly;

public class GameController
{
    private readonly Game _game;

    public GameController(Game game)
    {
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
        Console.WriteLine("Start of turn for {0}", current);
        Console.WriteLine("Cash=${0}, Net Worth=${1}", current.Cash, current.Appraise());

        if (current.JailTurns > 0)
        {
            current.JailTurns++;
        }

        OnTurnStarted(new GameEventArgs(_game));
        Propose(current);

        if (current.JailTurns > 0)
        {
            GetExitStrategy(current);
        }

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
        while (player.Agent.Propose() is not null)
            ;
    }

    private void GetExitStrategy(Player player)
    {
        switch (player.Agent.GetExitStrategy(_game, player.Id))
        {
            case JailExitStrategy.Bail:
                Charge(player, amount: 50);

                player.JailTurns = 0;

                break;

            case JailExitStrategy.Escape:
                player.Play(_game.Decks);
                break;

            default:
                break;
        }
    }

    private void Charge(Player player, int amount)
    {

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
