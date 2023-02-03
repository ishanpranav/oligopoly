namespace Oligopoly.Agents;

public class Agent
{
    private static Agent? s_instance;

    private Agent() { }

    public static Agent Default
    {
        get
        {
            if (s_instance is null)
            {
                s_instance = new Agent();
            }

            return s_instance;
        }
    }

    public void Connect(GameController controller) { }

    //public bool Purchase(Game game, Player player, Square property)
    //{
    //    return false;
    //}

    //public int Bid(Game game, Player player, PropertySquare property)
    //{
    //    return 0;
    //}

    //public IEnumerable<PropertySquare> Improve(Game game, Player player)
    //{
    //    return Enumerable.Empty<PropertySquare>();
    //}

    //public IEnumerable<PropertySquare> Unimprove(Game game, Player player)
    //{
    //    return Enumerable.Empty<PropertySquare>();
    //}

    //public IEnumerable<PropertySquare> Mortgage(Game game, Player player)
    //{
    //    return Enumerable.Empty<PropertySquare>();
    //}

    //public IEnumerable<PropertySquare> Unmortgage(Game game, Player player)
    //{
    //    return Enumerable.Empty<PropertySquare>();
    //}

    public JailExitStrategy GetExitStrategy(Game game, int player)
    {
        return JailExitStrategy.None;
    }

    public DealProposal? Propose()
    {
        return null;
    }

    public DealResponse Respond()
    {
        return DealResponse.Reject;
    }
}
