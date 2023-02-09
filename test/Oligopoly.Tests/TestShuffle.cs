using System.Collections.Generic;
using Oligopoly.Shuffles;

namespace Oligopoly.Tests;

internal sealed class TestShuffle : IShuffle
{
    private readonly int _cardId;

    public TestShuffle(int cardId)
    {
        _cardId = cardId;
    }

    /// <inheritdoc/>
    public void Shuffle(IList<int> items)
    {
        if (items.Count is 0)
        {
            return;
        }

        items[0] = _cardId;
        items[_cardId - 1] = 1;
    }
}
