using System.Collections.Generic;
using MessagePack;

namespace Oligopoly.Shuffles;

[Union(0, typeof(FisherYatesShuffle))]
public interface IShuffle
{
    void Shuffle<T>(IList<T> items);
}
