using System;
using Oligopoly.Writers;

namespace Oligopoly;

public class BoardSettings : IWritable
{
    public BoardSettings(int maxImprovements)
    {
        if (maxImprovements < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxImprovements));
        }

        MaxImprovements = maxImprovements;
    }

    public int MaxImprovements { get; }

    /// <inheritdoc/>
    public void Write(Writer writer)
    {
        writer.WriteVersion();
        writer.Write(MaxImprovements);
    }
}
