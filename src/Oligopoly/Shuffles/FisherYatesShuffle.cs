using System;
using System.Collections.Generic;
using MessagePack;

namespace Oligopoly.Shuffles;

[MessagePackObject]
public class FisherYatesShuffle : IShuffle
{
    private readonly Random _random;

    public FisherYatesShuffle()
    {
        _random = Random.Shared;
    }

    public FisherYatesShuffle(Random random)
    {
        _random = random;
    }

    /// <inheritdoc/>
    public void Shuffle(IList<int> items)
    {
        int n = items.Count;

        while (n > 1)
        {
            n--;

            int k = _random.Next(n + 1);
            int value = items[k];

            items[k] = items[n];
            items[n] = value;
        }
    }
}
