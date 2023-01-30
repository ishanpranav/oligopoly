namespace Oligopoly.Agents;

public class DealProposal
{
    private static DealProposal? s_emptyProposal;

    public DealProposal() { }

    public static DealProposal Empty
    {
        get
        {
            if (s_emptyProposal is null)
            {
                s_emptyProposal = new DealProposal();
            }

            return s_emptyProposal;
        }
    }
}
