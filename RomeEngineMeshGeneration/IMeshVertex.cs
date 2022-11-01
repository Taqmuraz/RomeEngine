using RomeEngine;

namespace RomeEngineMeshGeneration
{
    public interface IMeshVertex
    {
        void WriteElement(int index, IVertexBuffer buffer);
    }
}
