using MessagePack;

namespace Oligopoly.Auctions;

[MessagePackObject]
public readonly struct Bid
{
    public static readonly Bid Empty;

    public Bid(Player bidder, int amount)
    {
        Bidder = bidder;
        Amount = amount;
    }

    [Key(0)]
    public Player Bidder { get; }

    [Key(1)]
    public int Amount { get; }

    [IgnoreMember]
    public bool IsEmpty
    {
        get
        {
            return Amount == 0;
        }
    }
}
