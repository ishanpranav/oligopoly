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
    public void Shuffle<T>(IList<T> items)
    {

    }
}
