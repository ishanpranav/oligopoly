using System;
using System.Collections.Generic;
using MessagePack;
using Oligopoly.Cards;
using Oligopoly.Squares;

namespace Oligopoly;

[MessagePackObject]
public class Board
{
    public Board(IReadOnlyList<ISquare> squares, IReadOnlyList<Group> groups, IReadOnlyList<Deck> decks, int savings, int salary, int bail, double mortgageLoanProportion, double mortgageInterestRate, int railroadCost, int utilityCost, int speedLimit, int sentence)
    {
        foreach (ISquare square in squares)
        {
            if (square is CardSquare cardSquare)
            {
                cardSquare.Deck = decks[cardSquare.DeckId - 1];
            }
            else if (square is StreetSquare streetSquare)
            {
                streetSquare.Group = groups[streetSquare.GroupId - 1];
            }
        }

        for (int i = 0; i < decks.Count; i++)
        {
            Deck deck = decks[i];

            deck.Id = i + 1;

            for (int j = 0; j < deck.Cards.Count; j++)
            {
                ICard card = deck.Cards[j];

                card.Id = new CardId(deck.Id, j + 1);
            }
        }

        Squares = squares;
        Groups = groups;
        Decks = decks;
        Savings = savings;
        Salary = salary;
        MortgageLoanProportion = mortgageLoanProportion;
        MortgageInterestRate = mortgageInterestRate;
        RailroadCost = railroadCost;
        UtilityCost = utilityCost;
        SpeedLimit = speedLimit;
        Sentence = sentence;
    }

    [Key(0)]
    public IReadOnlyList<ISquare> Squares { get; }

    [Key(1)]
    public IReadOnlyList<Group> Groups { get; }

    [Key(2)]
    public IReadOnlyList<Deck> Decks { get; }

    [Key(4)]
    public int Savings { get; }

    [Key(5)]
    public int Salary { get; }

    [Key(6)]
    public int Bail { get; }

    [Key(7)]
    public double MortgageLoanProportion { get; }

    [Key(8)]
    public double MortgageInterestRate { get; }

    [Key(9)]
    public int RailroadCost { get; }

    [Key(10)]
    public int UtilityCost { get; }

    [Key(11)]
    public int SpeedLimit { get; }

    [Key(12)]
    public int Sentence { get; }

    public Player CreatePlayer(string name)
    {
        return new Player(name)
        {
            Cash = Savings
        };
    }
}
