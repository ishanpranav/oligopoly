using System;
using System.Collections.Generic;
using Oligopoly.Agents;

namespace Oligopoly;

public class GameController
{
    private readonly Game _game;

    public GameController(Game game)
    {
        _game = game;
    }

    public void Start()
    {
        Console.WriteLine("Start of game. Players:");

        foreach (Player player in _game.Players)
        {
            player.Agent.Start(_game);

            Console.WriteLine("\t{0}", player.Name);
        }

        int turns = 0;

        while (turns < int.MaxValue && !_game.Terminated)
        {
            foreach (Player player in new List<Player>(_game.Players))
            {
                StartTurn(player);

                if (_game.Terminated)
                {
                    break;
                }
            }

            turns++;
        }
    }

    private void StartTurn(Player player)
    {
        Console.WriteLine();
        Console.WriteLine("Start of turn for {0}", player);
        Console.WriteLine("Cash=${0}, Net Worth=${1}", player.Cash, player.Appraise());
    }

    private Roll Roll()
    {
        return new Roll(Random.Shared.Next(1, 7), Random.Shared.Next(1, 7));
    }
}
