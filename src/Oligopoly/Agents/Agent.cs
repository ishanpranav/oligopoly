using System.Collections.Generic;
using System.Linq;
using Oligopoly.Squares;

namespace Oligopoly.Agents;

public class Agent
{
    public Agent() { }

    public void Start(Game game)
    {

    }

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

    public JailExitStrategy GetExitStrategy(Game game, Player player)
    {
        return JailExitStrategy.None;
    }

    public DealProposal Propose()
    {
        return DealProposal.Empty;
    }

    public DealResponse Respond()
    {
        return DealResponse.Reject;
    }
}
