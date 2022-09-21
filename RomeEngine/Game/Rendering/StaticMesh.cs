using RomeEngine.IO;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public sealed class StaticMesh : IMesh, ISerializable
    {
        public StaticMesh()
        {
        }

        public StaticMesh(Vertex[] vertices, int[] indices)
        {
            Vertices = vertices;
            Indices = indices;
        }

        public Vertex[] Vertices { get; set; }
        public int[] Indices { get; set; }

        public IEnumerable<Vertex> EnumerateVertices()
        {
            return Vertices == null ? Enumerable.Empty<Vertex>() : Vertices;
        }

        public IEnumerable<int> EnumerateIndices()
        {
            return Indices == null ? Enumerable.Empty<int>() : Indices;
        }

        public static StaticMesh CreateBoxMesh()
        {
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(0f, 0f, 0f),
                new Vector3(1f, 0f, 0f),

                new Vector3(1f, 1f, 0f),
                new Vector3(0f, 1f, 0f),

                new Vector3(0f, 0f, 1f),
                new Vector3(1f, 0f, 1f),

                new Vector3(1f, 1f, 1f),
                new Vector3(0f, 1f, 1f),
            };

            int[] indices = Enumerable.Range(0, 8).ToArray();

            return new StaticMesh(vertices.Select(v => v - Vector3.one * 0.5f).Select(v => new Vertex(v, v, Vector2.zero)).ToArray(), indices);
        }

        public IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new SerializableField(nameof(Vertices), Vertices, v => Vertices = (Vertex[])v, typeof(Vertex[]), true);
            yield return new SerializableField(nameof(Indices), Indices, v => Indices = (int[])v, typeof(int[]), true);
        }
    }
}
