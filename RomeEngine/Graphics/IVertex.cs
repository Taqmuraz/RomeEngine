using RomeEngine.IO;
using System.Collections.Generic;

namespace RomeEngine
{
    public interface IVertex : ISerializable
    {
        IEnumerable<IVertexAttribute> Attributes { get; }
        int AttributesCount { get; }
    }
}
