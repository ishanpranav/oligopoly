using System;

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

    public DealProposal? Propose()
    {
        Console.WriteLine("AGENT: I do not have any deals to propose.");

        return null;
    }

    public DealResponse Respond()
    {
        Console.WriteLine("AGENT: I reject that offer.");

        return DealResponse.Reject;
    }

    public JailbreakStrategy Jailbreak(Game game, int player)
    {
        Console.WriteLine("AGENT: I would like to stay in jail.");

        return JailbreakStrategy.None;
    }

    public int Improve(Game game, Player player)
    {
        Console.WriteLine("AGENT: I do not want to improve anything.");

        return 0;
    }

    public int Unmortgage(Game game, Player player)
    {
        Console.WriteLine("AGENT: I do not want to unmortgage anything.");

        return 0;
    }

    public void Tax(int amount)
    {
        Console.WriteLine("AGENT: I recognize that I must pay {0} units of currency.", amount);
    }

    public void Taxed(int amount)
    {
        Console.WriteLine("AGENT: I pay {0} units of currency.", amount);
    }

    public void Untaxed(int amount)
    {
        Console.WriteLine("AGENT: I earn {0} units of currency.", amount);
    }

    public void Warn(Warning warning)
    {
        throw new InvalidOperationException(warning.ToString());
    }
}
