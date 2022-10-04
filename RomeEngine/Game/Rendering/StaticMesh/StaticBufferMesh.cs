using RomeEngine.IO;
using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class StaticBufferMesh : IMesh, ISourceObject
    {
        float[] vertices;
        float[] texcoords;
        float[] normals;
        int[] indices;


        float[][] Buffers => new float[][] { vertices, texcoords, normals };

        public StaticBufferMesh()
        {
            Attributes = new IMeshAttributeInfo[]
            {
                new CustomMeshAttribute(3, MeshAttributeType.Float),
                new CustomMeshAttribute(2, MeshAttributeType.Float),
                new CustomMeshAttribute(3, MeshAttributeType.Float),
            };
        }

        public PolygonFormat PolygonFormat { get; }

        public StaticBufferMesh(float[] vertices, float[] texcoords, float[] normals, int[] indices, PolygonFormat polygonFormat) : this()
        {
            this.vertices = vertices;
            this.texcoords = texcoords;
            this.normals = normals;
            this.indices = indices;
            PolygonFormat = polygonFormat;
        }

        public IEnumerable<int> EnumerateIndices()
        {
            return indices;
        }

        public ReadOnlyArray<IMeshAttributeInfo> Attributes { get; }

        public IVertexBuffer<float> CreateVerticesFloatAttributeBuffer(int attributeIndex)
        {
            return new VertexBuffer<float>(Buffers[attributeIndex]);
        }

        public IVertexBuffer<int> CreateVerticesIntAttributeBuffer(int attributeIndex)
        {
            throw new NotImplementedException();
        }

        public int PositionAttributeIndex => 0;
        public int TexcoordAttributeIndex => 1;
        public int NormalAttributeIndex => 2;

        public IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new GenericSerializableField<float[]>(nameof(vertices), vertices, value => vertices = value, true);
            yield return new GenericSerializableField<float[]>(nameof(texcoords), texcoords, value => texcoords = value, true);
            yield return new GenericSerializableField<float[]>(nameof(normals), normals, value => normals = value, true);
            yield return new GenericSerializableField<int[]>(nameof(indices), indices, value => indices = value, true);
        }

        ISerializable ISourceObject.CloneSourceReference() => this;
    }
}
