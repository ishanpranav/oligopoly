using System;
using System.IO;
using Oligopoly.Writers;

namespace Oligopoly;

public class Group : IWritable
{
    public Group() { }

    /// <inheritdoc/>
    public void Write(Writer writer)
    {
        
    }

    internal static Group Read(BinaryReader reader)
    {
        throw new NotImplementedException();
    }
}
