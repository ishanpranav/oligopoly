using System.Collections.Generic;
using MessagePack;
using Oligopoly.Cards;
using Oligopoly.Squares;

namespace Oligopoly;

[MessagePackObject]
public class Board
{
    public Board(IReadOnlyList<ISquare> squares, IReadOnlyList<Group> groups, IReadOnlyList<Deck> decks, int savings, int salary, int bail, double mortgageLoanProportion, double mortgageInterestRate, double unimprovementRate, int railroadCost, int utilityCost, int speedLimit, int sentence, int groupRentMultiplier, IReadOnlyList<int> railroadFares, IReadOnlyList<int> utilityBillMultipliers, int houses, int hotels)
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

                card.Id = new CardId(j + 1, deck.Id);
            }
        }

        Squares = squares;
        Groups = groups;
        Decks = decks;
        Savings = savings;
        Salary = salary;
        Bail = bail;
        MortgageLoanProportion = mortgageLoanProportion;
        MortgageInterestRate = mortgageInterestRate;
        UnimprovementRate = unimprovementRate;
        RailroadCost = railroadCost;
        UtilityCost = utilityCost;
        SpeedLimit = speedLimit;
        Sentence = sentence;
        GroupRentMultiplier = groupRentMultiplier;
        RailroadFares = railroadFares;
        UtilityBillMultipliers = utilityBillMultipliers;
        Houses = houses;
        Hotels = hotels;
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
    public double UnimprovementRate { get; }

    [Key(10)]
    public int RailroadCost { get; }

    [Key(11)]
    public int UtilityCost { get; }

    [Key(12)]
    public int SpeedLimit { get; }

    [Key(13)]
    public int Sentence { get; }

    [Key(14)]
    public int GroupRentMultiplier { get; }

    [Key(15)]
    public IReadOnlyList<int> RailroadFares { get; }

    [Key(16)]
    public IReadOnlyList<int> UtilityBillMultipliers { get; }

    [Key(17)]
    public int Houses { get; }

    [Key(18)]
    public int Hotels { get; }
}
