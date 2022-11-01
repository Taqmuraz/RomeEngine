using System.Collections.Generic;

namespace RomeEngineMeshGeneration
{
    public interface IMeshGenerationProvider
    {
        IMeshDataDescriptor Descriptor { get; }
        IMeshBuilder Builder { get; }
        IEnumerable<IMeshElementGenerator> Elements { get; }
    }
}
