using MessagePack;

namespace Oligopoly.Assets;

[MessagePackObject]
public class Deed : IAsset
{

    /// <inheritdoc/>
    public int Appraise()
    {
        return 0;
    }
}
