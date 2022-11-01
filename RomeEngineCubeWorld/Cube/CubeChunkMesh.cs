using RomeEngine;
using System.Collections.Generic;

namespace RomeEngineCubeWorld
{
    public sealed class CubeChunkMesh : IMesh
    {
        public static IMeshAttributeInfo[] ChunkMeshAttributes { get; } = new IMeshAttributeInfo[]
        {
            new CustomMeshAttribute(3, MeshAttributeType.Float),
            new CustomMeshAttribute(2, MeshAttributeType.Float),
            new CustomMeshAttribute(3, MeshAttributeType.Float),
        };

        int[] indices;
        IVertexBuffer[] buffers;

        public CubeChunkMesh(int[] indices, IVertexBuffer[] buffers)
        {
            this.indices = indices;
            this.buffers = buffers;
        }

        public IEnumerable<int> EnumerateIndices()
        {
            return indices;
        }

        public ReadOnlyArray<IMeshAttributeInfo> Attributes => ChunkMeshAttributes;

        public IVertexBuffer<float> CreateVerticesFloatAttributeBuffer(int attributeIndex)
        {
            var buffer = buffers[attributeIndex];
            return new VertexBuffer<float>((float[])buffer.ToArray());
        }

        public IVertexBuffer<int> CreateVerticesIntAttributeBuffer(int attributeIndex)
        {
            var buffer = buffers[attributeIndex];
            return new VertexBuffer<int>((int[])buffer.ToArray());
        }

        public int PositionAttributeIndex => 0;
        public int TexcoordAttributeIndex => 2;
        public int NormalAttributeIndex => 1;
        public PolygonFormat PolygonFormat => PolygonFormat.Triangles;
    }
}
