using System.Collections.Generic;

namespace RomeEngine
{
    public interface IMesh
    {
        IEnumerable<IVertex> EnumerateVertices();
        IEnumerable<int> EnumerateIndices();
    }
}
