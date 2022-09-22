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
                new Vector3(0f, 1f, 0f),

                new Vector3(1f, 1f, 0f),
                new Vector3(1f, 0f, 0f),

                new Vector3(1f, 0f, 1f),
                new Vector3(1f, 1f, 1f),

                new Vector3(0f, 1f, 1f),
                new Vector3(0f, 0f, 1f),
            };

            int[] indices = new int[]
            {
                0, 1, 2,
                2, 3, 0,
                3, 2, 4,
                2, 5, 4,
                4, 5, 6,
                7, 6, 1,
                6, 7, 4,
                1, 0, 7,
                7, 0, 3,
                3, 4, 7,
                1, 5, 2,
                1, 6, 5
            };

            return new StaticMesh(vertices.Select(v => v - Vector3.one * 0.5f).Select(v => new Vertex(v, v, Vector2.zero)).ToArray(), indices);
        }
        public static StaticMesh CreatePyramideMesh()
        {
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(0f, 0.5f, 0f),
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),
            };
            int[] indices = new int[]
            {
                0, 2, 1,
                0, 3, 2,
                0, 4, 3,
                0, 1, 4,
                1, 2, 3,
                3, 4, 1
            };
            return new StaticMesh(vertices.Select(v => new Vertex(v, v, Vector2.zero)).ToArray(), indices);
        }

        public IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new SerializableField(nameof(Vertices), Vertices, v => Vertices = (Vertex[])v, typeof(Vertex[]), true);
            yield return new SerializableField(nameof(Indices), Indices, v => Indices = (int[])v, typeof(int[]), true);
        }
    }
}
