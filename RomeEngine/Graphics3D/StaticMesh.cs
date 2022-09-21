using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public sealed class StaticMesh : IMesh
    {
        public Vertex[] Vertices { get; set; }

        public IEnumerable<Vertex> EnumerateVertices()
        {
            return Vertices == null ? Enumerable.Empty<Vertex>() : Vertices;
        }
    }
}
