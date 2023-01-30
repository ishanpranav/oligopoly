namespace Oligopoly;

internal readonly struct Roll
{
    public int First { get; }
    public int Second { get; }

    public int Result
    {
        get
        {
            return First + Second;
        }
    }

    public bool IsDouble
    {
        get
        {
            return First == Second;
        }
    }

    public Roll(int first, int second)
    {
        First = first;
        Second = second;
    }
}
