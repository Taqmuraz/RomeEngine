using System.Collections.Generic;

namespace RomeEngineMeshGeneration
{
    public interface IMeshStream
    {
        int PushStartIndex();
        void WriteIndices(IEnumerable<int> indices);
        void WriteVertices(IEnumerable<IMeshVertex> vertices);
    }
}
