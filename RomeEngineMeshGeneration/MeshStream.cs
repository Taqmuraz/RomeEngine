using System.Collections.Generic;
using System.Linq;
using RomeEngine;

namespace RomeEngineMeshGeneration
{
    public sealed class MeshStream : IMeshStream
    {
        List<int> indices = new List<int>();
        List<IMeshVertex> vertices = new List<IMeshVertex>();
        int lastIndex;

        public int PushStartIndex()
        {
            return lastIndex = vertices.Count;
        }

        public void WriteIndices(IEnumerable<int> indices)
        {
            this.indices.AddRange(indices.Select(i => i + lastIndex));
        }

        public void WriteVertices(IEnumerable<IMeshVertex> vertices)
        {
            this.vertices.AddRange(vertices);
        }

        public IMesh BuildMesh(IMeshDataDescriptor descriptor, IMeshBuilder builder)
        {
            var buffers = descriptor.Attributes.Select(a => new VertexBuffer(vertices.Count * a.Size, a.Type)).ToArray();

            foreach (var vertex in vertices)
            {
                for (int i = 0; i < buffers.Length; i++)
                {
                    vertex.WriteElement(i, buffers[i]);
                }
            }

            return builder.Build(indices.ToArray(), buffers);
        }
    }
}
