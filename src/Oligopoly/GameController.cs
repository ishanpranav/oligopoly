using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Oligopoly.Agents;
using Oligopoly.Auctions;
using Oligopoly.Cards;
using Oligopoly.EventArgs;
using Oligopoly.Squares;

namespace Oligopoly;

public class GameController
{
    private int _speed;
    private bool _proposing;

    public GameController(Board board, Game game)
    {
        Board = board;
        Game = game;
    }

    public event EventHandler<GameEventArgs>? Started;
    public event EventHandler<GameEventArgs>? TurnStarted;
    public event EventHandler<PlayerEventArgs>? Advanced;
    public event EventHandler<AuctionEventArgs>? AuctionSucceeded;
    public event EventHandler<AuctionEventArgs>? AuctionFailed;

    public Board Board { get; }
    public Game Game { get; }
    public int Dice { get; private set; }
    public IAuction Auction { get; set; } = new EnglishAuction();

    public void Start()
    {
        Console.WriteLine("Start of game. Players:");

        foreach (Player player in Game.Players)
        {
            player.Agent.Connect(controller: this);

            Console.WriteLine("\t{0}", player.Name);
        }

        OnStarted(new GameEventArgs(Game));
    }

    public bool MoveNext()
    {
        int i = Game.Turn % Game.Players.Count;
        Player player = Game.Players[i];

        if (player.Cash < 0)
        {
            return true;
        }

        Console.WriteLine();
        Console.WriteLine("Start of turn {0} for {1}", Game.Turn, player);
        Console.WriteLine("Cash=${0}, Net Worth=${1}", player.Cash, player.Appraise(Board, Game));

        if (player.Sentence > 0)
        {
            player.Sentence--;
        }

        OnTurnStarted(new GameEventArgs(Game));
        Propose(player);
        Jailbreak(player);
        Unmortgage(player);
        Improve(player);

        while (Roll(player))
            ;

        Console.WriteLine("End of turn {0} for {1}", Game.Turn, player);
        Console.WriteLine("Cash=${0}, Net Worth=${1}", player.Cash, player.Appraise(Board, Game));

        Game.Turn++;

        foreach (Player other in Game.Players)
        {
            if (other.Cash >= 0)
            {
                return true;
            }
        }

        return false;
    }

    private bool Roll(Player player)
    {
        int first = Game.Random.Next(1, 7);
        int second = Game.Random.Next(1, 7);
        bool result = first == second;

        Dice = first + second;

        Console.WriteLine("Rolled ({0}, {1})", first, second);

        if (player.Sentence is not 0)
        {
            if (result)
            {
                result = false;
                player.Sentence = 0;
            }
            else
            {
                if (player.Sentence > 0)
                {
                    return false;
                }

                Tax(player, Board.Bail);

                player.Sentence = 0;
            }
        }

        if (result)
        {
            _speed++;

            if (_speed == Board.SpeedLimit)
            {
                Police(player);

                return false;
            }
        }

        Travel(player, Dice);

        return result && player.Sentence is 0;
    }

    private void Jailbreak(Player player)
    {
        if (player.Sentence > 0)
        {
            switch (player.Agent.Jailbreak(Game, player))
            {
                case JailbreakStrategy.Bail:
                    Tax(player, Board.Bail);

                    player.Sentence = 0;

                    break;

                case JailbreakStrategy.Card:
                    if (player.CardIds.TryDequeue(out CardId cardId))
                    {
                        player.Sentence = 0;

                        Game.Discard(cardId);
                    }
                    else
                    {
                        Warn(player, Warning.InsufficientCards);
                    }

                    break;
            }
        }
    }

    public void Police(Player player)
    {
        int squareId = 0;

        player.Sentence = Board.Sentence;

        for (int i = 0; i < Board.Squares.Count; i++)
        {
            if (Board.Squares[i] is JailSquare)
            {
                squareId = i + 1;

                break;
            }
        }

        Advance(player, squareId);
    }

    public void Advance(Player player, int squareId)
    {
        player.SquareId = squareId;

        ISquare square = Board.Squares[squareId - 1];

        Console.WriteLine("Moved to {0}", square);

        OnAdvanced(new PlayerEventArgs(player));

        square.Advance(player, controller: this);
    }

    public void Travel(Player player, int distance)
    {
        if (distance is 0)
        {
            return;
        }

        int squareId = player.SquareId + distance;

        if (distance < 0)
        {
            while (squareId < 1)
            {
                squareId += Board.Squares.Count;
            }
        }
        else
        {
            while (squareId > Board.Squares.Count)
            {
                Console.WriteLine("{0} gets £{1} for passing the starting square", player, Board.Salary);

                squareId -= Board.Squares.Count;
                player.Cash += Board.Salary;
            }
        }

        Advance(player, squareId);
    }

    public bool Request(Player sender, Player recipient, int amount)
    {
        Tax(sender, amount);

        if (sender.Cash < 0)
        {
            Untax(sender, amount);

            return false;
        }
        else
        {
            Untax(recipient, amount);

            return true;
        }
    }

    public bool Demand(Player sender, Player recipient, int amount)
    {
        int actualAmount = Tax(sender, amount);

        if (sender.Cash < 0)
        {
            Untax(recipient, actualAmount);

            return false;
        }
        else
        {
            Untax(recipient, amount);

            return true;
        }
    }

    public int Tax(Player player, int amount)
    {
        player.Agent.Tax(Game, player, amount);
        Propose(player);
        Unimprove(player);
        Mortgage(player);

        int cash = player.Cash;

        player.Cash -= amount;
        player.Agent.Taxed(Game, player, amount);

        Console.WriteLine("{0} pays £{1}", player, amount);

        if (player.Cash >= 0)
        {
            return amount;
        }
        else
        {
            return cash;
        }
    }

    public void Untax(Player player, int amount)
    {
        player.Cash += amount;
        player.Agent.Untaxed(Game, player, amount);

        Console.WriteLine("{0} gets £{1}", player, amount);
    }

    private void Mortgage(Player player)
    {
        while (true)
        {
            int id = player.Agent.Mortgage(Game, player);

            if (id is 0)
            {
                break;
            }

            Deed deed = Game.Deeds[id - 1];

            if (deed.PlayerId != player.Id)
            {
                Warn(player, Warning.AccessDenied);

                break;
            }

            if (deed.Mortgaged)
            {
                Warn(player, Warning.Mortgaged);

                break;
            }

            if (deed.Improvements > 0)
            {
                Warn(player, Warning.Improved);

                break;
            }

            Untax(player, (int)(deed.Appraise(Board, Game) * Board.MortgageLoanProportion));

            deed.Mortgaged = true;
        }
    }

    private void Unmortgage(Player player)
    {
        while (true)
        {
            int id = player.Agent.Unmortgage(Game, player);

            if (id is 0)
            {
                break;
            }

            Deed deed = Game.Deeds[id - 1];

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

            int amount = (int)((Board.MortgageLoanProportion + Board.MortgageInterestRate) * deed.Appraise(Board, Game));

            Tax(player, amount);

            if (player.Cash < 0)
            {
                Untax(player, amount);
                Warn(player, Warning.InsufficientFunds);

                break;
            }

            deed.Mortgaged = true;
        }
    }

    private void Improve(Player player)
    {
        while (true)
        {
            int id = player.Agent.Improve(Game, player);

            if (id is 0)
            {
                break;
            }

            Console.WriteLine("{0} wants to build a house on {1}", player, id);

            Deed deed = Game.Deeds[id - 1];

            if (Board.Squares[id - 1] is not StreetSquare streetSquare)
            {
                Warn(player, Warning.NotImprovable);

                break;
            }

            deed.Improvements++;

            Group? group = streetSquare.Group!;

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
                Warn(player, Warning.Improved);

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

            foreach (KeyValuePair<int, Deed> deedId in Game.Deeds)
            {
                if (deedId.Key == id)
                {
                    continue;
                }

                if (Board.Squares[deedId.Key] is not StreetSquare other)
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
        while (true)
        {
            int id = player.Agent.Unimprove(Game, player);

            if (id is 0)
            {
                break;
            }

            Deed deed = Game.Deeds[id - 1];

            if (deed.Improvements <= 0)
            {
                Warn(player, Warning.Unimproved);

                break;
            }

            if (Board.Squares[id - 1] is not StreetSquare streetSquare)
            {
                Warn(player, Warning.NotImprovable);

                break;
            }

            int maxImprovements = deed.Improvements - 1;
            int minImprovements = deed.Improvements - 1;

            foreach (KeyValuePair<int, Deed> deedId in Game.Deeds)
            {
                if (deedId.Key == id)
                {
                    continue;
                }

                if (Board.Squares[deedId.Key] is not StreetSquare other)
                {
                    continue;
                }

                if (other.GroupId != streetSquare.GroupId)
                {
                    continue;
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
                Warn(player, Warning.UnbalancedImprovements);

                return;
            }

            deed.Improvements--;

            Untax(player, (int)(streetSquare.Group!.ImprovementCost * (1 + Board.AppreciationRate)));
        }
    }

    public void Offer(Player player, Deed deed)
    {
        PropertySquare propertySquare = (PropertySquare)Board.Squares[deed.SquareId - 1];

        if (player.Agent.Offer(Game, player, propertySquare))
        {
            int cost = propertySquare.Appraise(Board, Game);

            Tax(player, cost);

            if (player.Cash < 0)
            {
                Untax(player, cost);
            }
            else
            {
                deed.PlayerId = player.Id;

                return;
            }
        }

        Bid(deed, propertySquare);
    }

    public void Bid(Deed deed, IAsset asset)
    {
        Bid bid = Auction.Perform(controller: this, asset);

        if (bid.IsEmpty)
        {
            OnAuctionFailed(new AuctionEventArgs(asset));
        }
        else
        {
            Tax(bid.Bidder, bid.Amount);

            deed.PlayerId = bid.Bidder.Id;

            OnAuctionSucceeded(new AuctionEventArgs(asset, bid));
        }
    }

    private void Propose(Player player)
    {
        if (!_proposing)
        {
            _proposing = true;

            while (_proposing)
            {
                DealProposal? proposal = player.Agent.Propose(Game, player);

                if (proposal is null)
                {
                    _proposing = false;
                }
            }
        }
    }

    private void Warn(Player player, Warning warning)
    {
        player.Agent.Warn(Game, player, warning);

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

    protected virtual void OnAdvanced(PlayerEventArgs e)
    {
        if (Advanced is not null)
        {
            Advanced(sender: this, e);
        }
    }

    protected virtual void OnAuctionSucceeded(AuctionEventArgs e)
    {
        if (AuctionSucceeded is not null)
        {
            AuctionSucceeded(sender: this, e);
        }
    }

    protected virtual void OnAuctionFailed(AuctionEventArgs e)
    {
        if (AuctionFailed is not null)
        {
            AuctionFailed(sender: this, e);
        }
    }
}
