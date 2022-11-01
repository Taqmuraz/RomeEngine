using System.Collections.Generic;
using RomeEngine;

namespace RomeEngineMeshGeneration
{
    public interface IMeshBuilder
    {
        IMesh Build(int[] indices, IEnumerable<IVertexBuffer> buffers);
    }
}
