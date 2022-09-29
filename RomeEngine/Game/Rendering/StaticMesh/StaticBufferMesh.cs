using RomeEngine.IO;
using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class StaticBufferMesh : IMesh, ISerializable
    {
        int[] indices;
        MeshAttribute[] attributes;

        public StaticBufferMesh()
        {
        }

        public PolygonFormat PolygonFormat { get; }

        public StaticBufferMesh(MeshAttribute[] attributes, int[] indices, PolygonFormat polygonFormat) : this()
        {
            this.attributes = attributes;
            this.indices = indices;
            PolygonFormat = polygonFormat;
        }

        public IEnumerable<int> EnumerateIndices()
        {
            return indices;
        }

        public ReadOnlyArray<IMeshAttributeInfo> Attributes => attributes;

        public int PositionAttributeIndex => 0;
        public int TexcoordAttributeIndex => 1;
        public int NormalAttributeIndex => 2;

        public IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new GenericSerializableField<MeshAttribute[]>(nameof(attributes), attributes, value => attributes = value, true);
            yield return new GenericSerializableField<int[]>(nameof(indices), indices, value => indices = value, true);
        }

        public IVertexBuffer CreateVerticesAttributeBuffer(int attributeIndex)
        {
            return new VertexBuffer(attributes[attributeIndex].Type.ConvertFromAttributeBuffer(attributes[attributeIndex].Buffer));
        }
    }
}
