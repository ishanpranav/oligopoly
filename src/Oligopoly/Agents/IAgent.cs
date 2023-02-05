namespace Oligopoly.Agents;

public interface IAgent
{
    void Connect(GameController controller);
    void Tax(Game game, Player player, int amount);
    void Taxed(Game game, Player player, int amount);
    void Untaxed(Game game, Player player, int amount);
    int Mortgage(Game game, Player player);
    int Unmortgage(Game game, Player player);
    int Improve(Game game, Player player);
    int Unimprove(Game game, Player player);
    bool Offer(Game game, Player player, IAsset asset);
    int Bid(Game game, Player player, IAsset asset, int bid);
    DealProposal? Propose(Game game, Player player);
    JailbreakStrategy Jailbreak(Game game, Player player);
    void Warn(Game game, Player player, Warning warning);
}
