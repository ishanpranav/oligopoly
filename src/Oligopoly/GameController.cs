using System;
using System.Collections.Generic;
using Oligopoly.Agents;
using Oligopoly.Cards;
using Oligopoly.EventArgs;
using Oligopoly.Squares;

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
    public event EventHandler<PlayerEventArgs>? Landed;

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
        Console.WriteLine("Cash=${0}, Net Worth=${1}", current.Cash, current.Appraise(_board, _game));

        if (current.Sentence > 0)
        {
            current.Sentence--;
        }

        OnTurnStarted(new GameEventArgs(_game));
        Propose(current);
        Jailbreak(current);
        Unmortgage(current);
        Improve(current);

        int count = 0;
        bool isDouble = true;

        while (isDouble)
        {
            int first = Random.Shared.Next(1, 7);
            int second = Random.Shared.Next(1, 7);
            int result = first + second;

            isDouble = first == second;

            Console.WriteLine("Rolled ({0}, {1})", first, second);

            if (current.Sentence != 0)
            {
                if (isDouble)
                {
                    isDouble = false;
                    current.Sentence = 0;
                }
                else
                {
                    if (current.Sentence > 0)
                    {
                        break;
                    }

                    Tax(current, _board.Bail);

                    current.Sentence = 0;
                }
            }

            if (isDouble)
            {
                count++;

                if (count == _board.SpeedLimit)
                {
                    Police(current);

                    break;
                }
            }

            current.SquareId += result;

            while (current.SquareId >= _board.Squares.Count)
            {
                Console.WriteLine("{0} gets £{1} for passing the starting square", current, _board.Salary);

                current.SquareId -= _board.Squares.Count;
                current.Cash += _board.Salary;
            }

            Land(current);

            if (current.Sentence > 0)
            {
                break;
            }
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

    private void Land(Player player)
    {
        ISquare square = _board.Squares[player.SquareId];

        Console.WriteLine("Moved to {0}", square);

        OnLanded(new PlayerEventArgs(player));

        square.Land(player);
    }

    private void Police(Player player)
    {
        player.Sentence = _board.Sentence;

        for (int i = 0; i < _board.Squares.Count; i++)
        {
            if (_board.Squares[i] is JailSquare)
            {
                player.SquareId = i + 1;
            }
        }

        Land(player);
    }

    private void Propose(Player player)
    {
        if (!_proposing)
        {
            _proposing = true;

            while (_proposing)
            {
                DealProposal? proposal = player.Agent.Propose();

                if (proposal is null)
                {
                    _proposing = false;
                }
            }
        }
    }

    private void Jailbreak(Player player)
    {
        if (player.Sentence > 0)
        {
            switch (player.Agent.Jailbreak(_game, player.Id))
            {
                case JailbreakStrategy.Bail:
                    Tax(player, _board.Bail);

                    player.Sentence = 0;

                    break;

                case JailbreakStrategy.Card:
                    if (player.TryPlay(out CardId cardId))
                    {
                        player.Sentence = 0;

                        _game.Discard(cardId);
                    }

                    break;
            }
        }
    }

    private void Improve(Player player)
    {
        while (true)
        {
            int id = player.Agent.Unmortgage(_game, player);

            if (id is 0)
            {
                break;
            }

            Console.WriteLine("{0} wants to build a house on {1}", player, id);

            Deed deed = _game.Deeds[id - 1];

            if (_board.Squares[id - 1] is not StreetSquare streetSquare)
            {
                Warn(player, Warning.NotImprovable);

                break;
            }

            Group? group = streetSquare.Group!;

            deed.Improvements++;

            Tax(player, group.ImprovementCost);

            if (player.Cash < 0)
            {
                deed.Improvements--;

                Untax(player, group.ImprovementCost);
                Warn(player, Warning.InsufficientFunds);

                break;
            }

            if (deed.Improvements >= streetSquare.Rents.Count - 1)
            {
                Untax(player, group.ImprovementCost);
                Warn(player, Warning.MaxImprovementsExceeded);

                break;
            }

            if (deed.PlayerId != player.Id)
            {
                Untax(player, group.ImprovementCost);
                Warn(player, Warning.AccessDenied);

                break;
            }

            if (deed.Mortgaged)
            {
                Untax(player, group.ImprovementCost);
                Warn(player, Warning.Mortgaged);

                break;
            }

            int maxImprovements = deed.Improvements;
            int minImprovements = deed.Improvements;

            foreach (KeyValuePair<int, Deed> deedId in _game.Deeds)
            {
                if (deedId.Key == id)
                {
                    continue;
                }

                if (_board.Squares[deedId.Key] is not StreetSquare other)
                {
                    continue;
                }

                if (other.GroupId != streetSquare.GroupId)
                {
                    continue;
                }

                if (deedId.Value.PlayerId != player.Id)
                {
                    Untax(player, group.ImprovementCost);
                    Warn(player, Warning.GroupAccessDenied);

                    return;
                }

                if (deedId.Value.Mortgaged)
                {
                    Untax(player, group.ImprovementCost);
                    Warn(player, Warning.GroupAccessDenied);

                    return;
                }

                if (deedId.Value.Improvements > maxImprovements)
                {
                    maxImprovements = deedId.Value.Improvements;
                }

                if (deedId.Value.Improvements < minImprovements)
                {
                    minImprovements = deedId.Value.Improvements;
                }
            }

            if (maxImprovements - minImprovements > 1)
            {
                Untax(player, group.ImprovementCost);
                Warn(player, Warning.UnbalancedImprovements);

                return;
            }
        }
    }

    private void Unimprove(Player player)
    {

    }

    private void Mortgage(Player player)
    {

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

            Deed deed = _game.Deeds[deedId - 1];

            if (deed.PlayerId != player.Id)
            {
                Warn(player, Warning.AccessDenied);

                break;
            }

            if (!deed.Mortgaged)
            {
                Warn(player, Warning.Unmortgaged);

                break;
            }

            int amount = (int)((_board.MortgageLoanProportion + _board.MortgageInterestRate) * deed.Appraise(_board, _game));

            Tax(player, amount);

            if (player.Cash < 0)
            {
                Untax(player, amount);
                Warn(player, Warning.InsufficientFunds);

                break;
            }

            _game.Deeds[deedId - 1].Mortgaged = true;
        }
    }

    private void Tax(Player player, int amount)
    {
        player.Agent.Tax(amount);
        Propose(player);
        Unimprove(player);
        Mortgage(player);

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

    private void Warn(Player player, Warning warning)
    {
        Console.WriteLine("WARNING to {0}: {1}", player, warning);
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

    protected virtual void OnLanded(PlayerEventArgs e)
    {
        if (Landed is not null)
        {
            Landed(sender: this, e);
        }
    }
}
