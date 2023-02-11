namespace Oligopoly.Assets;

public interface IAsset : IAppraisable
{
    int GetPlayerId(Game game);
    bool Transfer(GameController controller, Player sender, Player recipient);
    void Discard(GameController controller, Player player);
}
