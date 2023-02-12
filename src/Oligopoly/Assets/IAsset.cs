namespace Oligopoly.Assets;

public interface IAsset : IAppraisable
{
    int GetPlayerId(Game game);
    string GetDescription(Board board);
    void Transfer(Controller controller, Player sender, Player recipient);
    void Discard(Controller controller, Player player);
}
