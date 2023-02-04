namespace Oligopoly.Squares;

public interface IPropertySquare : IAsset, ISquare
{
    int GetRent(Board board, Roll roll);
}
