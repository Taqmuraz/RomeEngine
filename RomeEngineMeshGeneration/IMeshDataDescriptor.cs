using System.Collections.Generic;
using RomeEngine;

namespace RomeEngineMeshGeneration
{
    public interface IMeshDataDescriptor
    {
        IEnumerable<IMeshAttributeInfo> Attributes { get; }
    }
}
