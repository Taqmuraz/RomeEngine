using RomeEngine.IO;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public sealed class StaticMesh : IMesh, ISerializable
    {
        public StaticMesh()
        {
            attributesFloat = new IStaticMeshAttribute<float>[]
            {
                new VertexAttributePosition(),
                new VertexAttributeUV(),
                new VertexAttributeNormal()
            };
            attributesInt = new IStaticMeshAttribute<int>[]
            {
            };

            Attributes = attributesFloat.Concat<IMeshAttributeInfo>(attributesInt).ToArray();
        }

        public StaticMesh(Vertex[] vertices, int[] indices) : this()
        {
            Vertices = vertices;
            Indices = indices;
        }

        public PolygonFormat PolygonFormat => PolygonFormat.Triangles;
        public Vertex[] Vertices { get; set; }
        public int[] Indices { get; set; }

        public IEnumerable<int> EnumerateIndices()
        {
            return Indices == null ? Enumerable.Empty<int>() : Indices;
        }

        public static StaticMesh CreateBoxMesh()
        {
            Vertex[] vertices = new Vertex[]
            {
                    new Vertex
                    (
                        new Vector3(0f, 0f, 0f),
                        new Vector3(0f, 0f, 0f),
                        new Vector2(0f, 0f)
                    ),
                    new Vertex
                    (
                        new Vector3(0f, 1f, 0f),
                        new Vector3(0f, 0f, 0f),
                        new Vector2(0f, 1f)
                    ),
                    new Vertex
                    (
                        new Vector3(1f, 1f, 0f),
                        new Vector3(0f, 0f, 0f),
                        new Vector2(1f, 1f)
                    ),
                    new Vertex
                    (
                        new Vector3(1f, 0f, 0f),
                        new Vector3(0f, 0f, 0f),
                        new Vector2(1f, 0f)
                    ),
                    new Vertex
                    (
                        new Vector3(1f, 0f, 1f),
                        new Vector3(0f, 0f, 0f),
                        new Vector2(1f, 0f)
                    ),
                    new Vertex
                    (
                        new Vector3(1f, 1f, 1f),
                        new Vector3(0f, 0f, 0f),
                        new Vector2(1f, 1f)
                    ),
                    new Vertex
                    (
                        new Vector3(0f, 1f, 1f),
                        new Vector3(0f, 0f, 0f),
                        new Vector2(0f, 1f)
                    ),
                    new Vertex
                    (
                        new Vector3(0f, 0f, 1f),
                        new Vector3(0f, 0f, 0f),
                        new Vector2(0f, 0f)
                    ),
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
            foreach (var vert in vertices) vert.Position -= Vector3.one * 0.5f;
            var result = new StaticMesh(vertices, indices);
            result.RecalculateNormals();
            return result;
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
            var result = new StaticMesh(vertices.Select(v => new Vertex(v, Vector3.zero, Vector2.zero)).ToArray(), indices);
            result.RecalculateNormals();
            return result;
        }

        public void RecalculateNormals()
        {
            void BlendNormal(Vertex vertex, Vector3 normal)
            {
                vertex.Normal = ((vertex.Normal + normal) * 0.5f).normalized;
            }

            for (int i = 0; i < Vertices.Length; i++) Vertices[i].Normal = Vector3.zero;

            for (int i = 2; i < Indices.Length; i+=3)
            {
                Vector3 t0 = Vertices[Indices[i - 2]].Position;
                Vector3 t1 = Vertices[Indices[i - 1]].Position;
                Vector3 t2 = Vertices[Indices[i]].Position;

                Vector3 normal = Vector3.Cross(t2 - t1, t0 - t1).normalized;

                BlendNormal(Vertices[Indices[i - 2]], normal);
                BlendNormal(Vertices[Indices[i - 1]], normal);
                BlendNormal(Vertices[Indices[i]], normal);
            }
        }

        public IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new SerializableField(nameof(Vertices), Vertices, v => Vertices = (Vertex[])v, typeof(Vertex[]), true);
            yield return new SerializableField(nameof(Indices), Indices, v => Indices = (int[])v, typeof(int[]), true);
        }

        IStaticMeshAttribute<float>[] attributesFloat;
        IStaticMeshAttribute<int>[] attributesInt;
        
        public ReadOnlyArray<IMeshAttributeInfo> Attributes { get; }

        void WriteVerticesToAttributeBuffer(IVertexBuffer<float> buffer, int attributeIndex)
        {
            foreach (var vertex in Vertices) ((IStaticMeshAttribute<float>)Attributes[attributeIndex]).WriteVertex(buffer, vertex);
        }
        void WriteVerticesToAttributeBuffer(IVertexBuffer<int> buffer, int attributeIndex)
        {
            foreach (var vertex in Vertices) ((IStaticMeshAttribute<int>)Attributes[attributeIndex]).WriteVertex(buffer, vertex);
        }

        int CalculateBufferSize(int index)
        {
            return Attributes[index].Size * Vertices.Length;
        }

        public IVertexBuffer<float> CreateVerticesFloatAttributeBuffer(int attributeIndex)
        {
            var buffer = new VertexBuffer<float>(CalculateBufferSize(attributeIndex));
            WriteVerticesToAttributeBuffer(buffer, attributeIndex);
            return buffer;
        }
        public IVertexBuffer<int> CreateVerticesIntAttributeBuffer(int attributeIndex)
        {
            var buffer = new VertexBuffer<int>(CalculateBufferSize(attributeIndex));
            WriteVerticesToAttributeBuffer(buffer, attributeIndex);
            return buffer;
        }

        public int PositionAttributeIndex => 0;
        public int TexcoordAttributeIndex => 1;
        public int NormalAttributeIndex => 2;
    }
}
