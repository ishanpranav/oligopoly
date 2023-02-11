using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Oligopoly.Squares;

namespace Oligopoly.Tests;

[TestClass]
public class BoardTest
{
    [TestMethod("Squares (1)")]
    public void TestSquaresCount()
    {
        Assert.AreEqual(40, Factory.CreateBoard().Squares.Count);
    }

    [DataRow("GO", 1, DisplayName = "Squares (2)")]
    [DataRow("Old Kent Road", 2, DisplayName = "Squares (3)")]
    [DataRow("Community Chest", 3, 18, 34, DisplayName = "Squares (4)")]
    [DataRow("Chance", 8, 23, 37, DisplayName = "Squares (5)")]
    [DataRow("Marylebone Station", 16, DisplayName = "Squares (6)")]
    [DataRow("Trafalgar Square", 25, DisplayName = "Squares (7)")]
    [DataRow("Mayfair", 40, DisplayName = "Squares (8)")]
    [DataTestMethod]
    public void TestSquares(string name, params int[] expectedIds)
    {
        Board board = Factory.CreateBoard();
        List<int> actualIds = new List<int>();

        for (int i = 0; i < board.Squares.Count; i++)
        {
            if (board.Squares[i].Name != name)
            {
                continue;
            }

            actualIds.Add(i + 1);
        }

        CollectionAssert.AreEquivalent(expectedIds, actualIds);
    }

    [DataRow(1, "Old Kent Road", "Whitechapel Road", DisplayName = "Squares (9)")]
    [DataRow(2, "The Angel Islington", "Euston Road", "Pentonville Road", DisplayName = "Squares (10)")]
    [DataRow(3, "Pall Mall", "Whitehall", "Northumberland Avenue", DisplayName = "Squares (11)")]
    [DataRow(4, "Bow Street", "Marlborough Street", "Vine Street", DisplayName = "Squares (12)")]
    [DataRow(5, "Strand", "Fleet Street", "Trafalgar Square", DisplayName = "Squares (13)")]
    [DataRow(6, "Leicester Square", "Coventry Street", "Piccadilly", DisplayName = "Squares (14)")]
    [DataRow(7, "Regent Street", "Oxford Street", "Bond Street", DisplayName = "Squares (15)")]
    [DataRow(8, "Park Lane", "Mayfair", DisplayName = "Squares (16)")]
    [DataTestMethod]
    public void TestGroups(int groupId, params string[] expectedNames)
    {
        Board board = Factory.CreateBoard();
        List<string> actualNames = new List<string>();

        for (int i = 0; i < board.Squares.Count; i++)
        {
            if (board.Squares[i] is not StreetSquare streetSquare)
            {
                continue;
            }

            if (streetSquare.GroupId != groupId)
            {
                continue;
            }

            actualNames.Add(streetSquare.Name);
        }

        CollectionAssert.AreEquivalent(expectedNames, actualNames);
    }
}
