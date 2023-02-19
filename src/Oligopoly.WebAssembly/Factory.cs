using Oligopoly.Dice;
using Oligopoly.Shuffles;

namespace Oligopoly;

internal static class Factory
{
    private static Board? s_board;
    private static D6PairDice? s_dice;
    private static FisherYatesShuffle? s_shuffle;

    public static Board Board
    {
        get
        {
            if (s_board is null)
            {
                string messagePackPath = "../../../../../data/board.bin";

                using FileStream input = File.OpenRead(messagePackPath);

                s_board = OligopolySerializer.ReadBoard(input);
            }

            return s_board;
        }
    }

    public static Game CreateGame(Board board, IDice? dice = null, IShuffle? shuffle = null)
    {
        if (dice is null)
        {
            if (s_dice is null)
            {
                s_dice = new D6PairDice();
            }

            dice = s_dice;
        }

        if (shuffle is null)
        {
            if (s_shuffle is null)
            {
                s_shuffle = new FisherYatesShuffle();
            }

            shuffle = s_shuffle;
        }

        return new Game(board.Squares, board.Decks, dice, shuffle)
        {
            Houses = board.Houses,
            Hotels = board.Hotels
        };
    }

    public static Controller CreateController()
    {
        Board board = Board;

        return new Controller(board, CreateGame(board));
    }
}
