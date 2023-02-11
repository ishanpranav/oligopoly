namespace Oligopoly;

public class Bid
{
    public Bid(Player bidder, int amount)
    {
        Bidder = bidder;
        Amount = amount;
    }

    public Player Bidder { get; }
    public int Amount { get; }
}
