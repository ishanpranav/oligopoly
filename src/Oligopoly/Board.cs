using System;
using System.Collections.Generic;
using MessagePack;
using Oligopoly.Cards;
using Oligopoly.Squares;

namespace Oligopoly;

[MessagePackObject]
public class Board
{
    public Board(IReadOnlyList<ISquare> squares, IReadOnlyList<Group> groups, IReadOnlyList<Deck> decks, int initialCash)
    {
        ArgumentNullException.ThrowIfNull(squares);
        ArgumentNullException.ThrowIfNull(groups);
        ArgumentNullException.ThrowIfNull(decks);

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

            deck.Id = i;

            for (int j = 0; j < deck.Cards.Count; j++)
            {
                Card card = deck.Cards[j];

                card.Id = j;
                card.DeckId = i;
            }
        }

        Squares = squares;
        Groups = groups;
        Decks = decks;
        InitialCash = initialCash;
    }

    [Key(0)]
    public IReadOnlyList<ISquare> Squares { get; }

    [Key(1)]
    public IReadOnlyList<Group> Groups { get; }

    [Key(2)]
    public IReadOnlyList<Deck> Decks { get; }

    [Key(4)]
    public int InitialCash { get; }
}
