namespace Oligopoly.Assets;

public interface IAsset : IAppraisable
{
    int GetPlayerId(Game game);
    string GetDescription(Board board);
    void Transfer(GameController controller, Player sender, Player recipient);
    void Discard(GameController controller, Player player);
}
