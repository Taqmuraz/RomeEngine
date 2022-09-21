using System.Collections.Generic;

namespace RomeEngine
{
    public interface IMesh
    {
        IEnumerable<Vertex> EnumerateVertices();
    }
}
