using Oligopoly.Assets;

namespace Oligopoly.Agents;

public interface IAgent
{
    void Connect(Controller controller);
    void OnTaxing(Game game, Player player, int amount);
    void OnTaxed(Game game, Player player, int amount);
    void OnUntaxed(Game game, Player player, int amount);
    int Mortgage(Game game, Player player);
    int Unmortgage(Game game, Player player);
    int Improve(Game game, Player player);
    int Unimprove(Game game, Player player);
    bool Offer(Game game, Player player, IAsset asset);
    int Bid(Game game, Player player, Offer offer);
    Offer? Propose(Game game, Player player);
    bool Respond(Game game, Player player);
    UnpoliceStrategy Unpolice(Game game, Player player);
    void Warn(Game game, Player player, Warning warning);
}
