namespace Oligopoly.Agents;

public class DealResponse
{
    private static DealResponse? s_rejectResponse;

    private readonly bool _accepted;
    
    public DealResponse(bool accepted)
    {
        _accepted = accepted;
    }

    public static DealResponse Reject
    {
        get
        {
            if (s_rejectResponse is null)
            {
                s_rejectResponse = new DealResponse(accepted: false);
            }

            return s_rejectResponse;
        }
    }
}
