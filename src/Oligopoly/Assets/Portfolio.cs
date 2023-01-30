using System.Collections.Generic;

namespace Oligopoly.Assets;

public class Portfolio : IAsset
{
    //private readonly HashSet<Deed> _deeds = new HashSet<Deed>();

    public int Cash { get; set; }

    /// <inheritdoc/>
    public int Appraise()
    {
        int result = Cash;

        //foreach (Deed deed in _deeds)
        //{
        //    result += deed.Appraise();
        //}

        return result;
    }
}
