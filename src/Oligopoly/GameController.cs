using System;
using System.Collections.Generic;
using Oligopoly.Agents;
using Oligopoly.Auctions;
using Oligopoly.Cards;
using Oligopoly.EventArgs;
using Oligopoly.Squares;

namespace Oligopoly;

public class GameController
{
    private int _id;
    private bool _proposing;

    public GameController(Board board, Game game)
    {
        Board = board;
        Game = game;
    }

    public event EventHandler<GameEventArgs>? Started;
    public event EventHandler<GameEventArgs>? Ended;
    public event EventHandler<GameEventArgs>? TurnStarted;
    public event EventHandler<GameEventArgs>? TurnEnded;
    public event EventHandler<PlayerEventArgs>? Advanced;
    public event EventHandler<AuctionEventArgs>? AuctionSucceeded;
    public event EventHandler<AuctionEventArgs>? AuctionFailed;

    public Board Board { get; }
    public Game Game { get; }
    public bool Flying { get; private set; }
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

    public Player AddPlayer(string name)
    {
        _id++;

        Player result = new Player(_id, name)
        {
            Cash = Board.Savings,
            SquareId = 1
        };

        Game.Players.Add(result);

        return result;
    }

    public bool MoveNext()
    {
        Player[] players = new Player[Game.Players.Count];

        Game.Players.CopyTo(players, arrayIndex: 0);
        Array.Sort(players);

        foreach (Player player in players)
        {
            if (player.Cash < 0)
            {
                continue;
            }

            Move(player);

            foreach (Player other in players)
            {
                if (other.Cash < 0)
                {
                    Bankrupt(other);
                    Game.Players.Remove(other);
                }
            }

            if (Game.Players.Count < 2)
            {
                OnEnded(new GameEventArgs(Game));

                return false;
            }
        }

        return true;
    }

    public void Move(Player player)
    {
        Console.WriteLine();
        Console.WriteLine("{0}: Cash=${1}, Net Worth=${2}, Sentence={3}, Square={4}", player, player.Cash, player.Appraise(Board, Game), player.Sentence, player.SquareId);

        GameEventArgs e = new GameEventArgs(Game);

        OnTurnStarted(e);
        Propose(player);
        Jailbreak(player);
        Unmortgage(player);
        Improve(player);

        int speed = 0;

        while (Game.Dice.Roll(controller: this, player))
        {
            speed++;

            if (speed == Board.SpeedLimit)
            {
                Police(player);

                break;
            }
        }

        Console.WriteLine("{0}: Cash=${1}, Net Worth=${2}, Sentence={3}, Square={4}", player, player.Cash, player.Appraise(Board, Game), player.Sentence, player.SquareId);
        OnTurnEnded(e);
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
        player.Sentence = Board.Sentence;

        for (int squareId = 1; squareId <= Board.Squares.Count; squareId++)
        {
            if (Board.Squares[squareId - 1] is JailSquare)
            {
                Advance(player, squareId);

                break;
            }
        }
    }

    public void Advance(Player player, int squareId)
    {
        player.SquareId = squareId;

        ISquare square = Board.Squares[squareId - 1];

        Console.WriteLine("Advanced to {0}", square);

        OnAdvanced(new PlayerEventArgs(player));

        square.Advance(player, controller: this);
    }

    public void Fly(Player player, int squareId)
    {
        Flying = true;

        Advance(player, squareId);

        Flying = false;
    }

    public void Jump(Player player, int distance)
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
                squareId -= Board.Squares.Count;
                player.Cash += Board.Salary;
            }
        }

        Advance(player, squareId);
    }

    public bool Gift(Player debtor, Player creditor, int amount)
    {
        int actualAmount = Tax(debtor, amount);

        if (debtor.Cash < 0)
        {
            Untax(creditor, actualAmount);
            Bankrupt(debtor, creditor);

            return false;
        }
        else
        {
            Untax(creditor, amount);

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

        if (player.Cash < 0)
        {
            return cash;
        }

        return amount;
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
                Warn(player, Warning.InsufficientCash);

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

            if (Board.Houses <= 0)
            {
                Warn(player, Warning.HouseShortage);

                return;
            }

            if (Board.Hotels <= 0)
            {
                Warn(player, Warning.HotelShortage);

                return;
            }

            Deed deed = Game.Deeds[id - 1];

            if (Board.Squares[id - 1] is not StreetSquare streetSquare)
            {
                Warn(player, Warning.NotImprovable);

                break;
            }

            deed.Improve(Game, streetSquare);

            Group? group = streetSquare.Group!;

            Tax(player, group.ImprovementCost);

            if (player.Cash < 0)
            {
                deed.Unimprove(Game, streetSquare);
                Untax(player, group.ImprovementCost);
                Warn(player, Warning.InsufficientCash);

                break;
            }

            if (deed.Improvements >= streetSquare.Rents.Count)
            {
                deed.Unimprove(Game, streetSquare);
                Untax(player, group.ImprovementCost);
                Warn(player, Warning.Improved);

                break;
            }

            if (deed.PlayerId != player.Id)
            {
                deed.Unimprove(Game, streetSquare);
                Untax(player, group.ImprovementCost);
                Warn(player, Warning.AccessDenied);

                break;
            }

            if (deed.Mortgaged)
            {
                deed.Unimprove(Game, streetSquare);
                Untax(player, group.ImprovementCost);
                Warn(player, Warning.Mortgaged);

                break;
            }

            int maxImprovements = deed.Improvements;
            int minImprovements = deed.Improvements;

            foreach (KeyValuePair<int, Deed> indexedDeed in Game.Deeds)
            {
                if (indexedDeed.Key == id - 1)
                {
                    continue;
                }

                if (Board.Squares[indexedDeed.Key] is not StreetSquare other)
                {
                    continue;
                }

                if (other.GroupId != streetSquare.GroupId)
                {
                    continue;
                }

                if (indexedDeed.Value.PlayerId != player.Id)
                {
                    deed.Unimprove(Game, streetSquare);
                    Untax(player, group.ImprovementCost);
                    Warn(player, Warning.GroupAccessDenied);

                    return;
                }

                if (indexedDeed.Value.Mortgaged)
                {
                    deed.Unimprove(Game, streetSquare);
                    Untax(player, group.ImprovementCost);
                    Warn(player, Warning.GroupMortgaged);

                    return;
                }

                if (indexedDeed.Value.Improvements > maxImprovements)
                {
                    maxImprovements = indexedDeed.Value.Improvements;
                }

                if (indexedDeed.Value.Improvements < minImprovements)
                {
                    minImprovements = indexedDeed.Value.Improvements;
                }
            }

            if (maxImprovements - minImprovements > 1)
            {
                deed.Unimprove(Game, streetSquare);
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

            foreach (KeyValuePair<int, Deed> indexedDeed in Game.Deeds)
            {
                if (indexedDeed.Key == id - 1)
                {
                    continue;
                }

                if (Board.Squares[indexedDeed.Key] is not StreetSquare other)
                {
                    continue;
                }

                if (other.GroupId != streetSquare.GroupId)
                {
                    continue;
                }

                if (indexedDeed.Value.Improvements > maxImprovements)
                {
                    maxImprovements = indexedDeed.Value.Improvements;
                }

                if (indexedDeed.Value.Improvements < minImprovements)
                {
                    minImprovements = indexedDeed.Value.Improvements;
                }
            }

            if (maxImprovements - minImprovements > 1)
            {
                Warn(player, Warning.UnbalancedImprovements);

                return;
            }

            deed.Unimprove(Game, streetSquare);
            Untax(player, (int)(streetSquare.Group!.ImprovementCost * (1 - Board.UnimprovementRate)));
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

    private void Bankrupt(Player debtor, Player creditor)
    {
        foreach (KeyValuePair<int, Deed> indexedDeed in Game.Deeds)
        {
            if (indexedDeed.Value.PlayerId != debtor.Id)
            {
                continue;
            }

            if (indexedDeed.Value.Mortgaged)
            {
                Tax(creditor, (int)(Board.MortgageInterestRate * indexedDeed.Value.Appraise(Board, Game)));

                indexedDeed.Value.PlayerId = creditor.Id;

                Unmortgage(creditor);
            }
            else
            {
                indexedDeed.Value.PlayerId = creditor.Id;
            }

            if (Board.Squares[indexedDeed.Key] is StreetSquare streetSquare)
            {
                while (indexedDeed.Value.Improvements > 0)
                {
                    indexedDeed.Value.Unimprove(Game, streetSquare);
                }
            }
        }

        while (debtor.CardIds.TryDequeue(out CardId cardId))
        {
            creditor.CardIds.Enqueue(cardId);
        }
    }

    private void Bankrupt(Player player)
    {
        foreach (KeyValuePair<int, Deed> indexedDeed in Game.Deeds)
        {
            if (indexedDeed.Value.PlayerId != player.Id)
            {
                continue;
            }

            indexedDeed.Value.PlayerId = 0;
            indexedDeed.Value.Mortgaged = false;

            if (Board.Squares[indexedDeed.Key] is StreetSquare streetSquare)
            {
                while (indexedDeed.Value.Improvements > 0)
                {
                    indexedDeed.Value.Unimprove(Game, streetSquare);
                }
            }
        }

        while (player.CardIds.TryDequeue(out CardId cardId))
        {
            Game.Discard(cardId);
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

    protected virtual void OnEnded(GameEventArgs e)
    {
        if (Ended is not null)
        {
            Ended(sender: this, e);
        }
    }

    protected virtual void OnTurnStarted(GameEventArgs e)
    {
        if (TurnStarted is not null)
        {
            TurnStarted(sender: this, e);
        }
    }

    protected virtual void OnTurnEnded(GameEventArgs e)
    {
        if (TurnEnded is not null)
        {
            TurnEnded(sender: this, e);
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
