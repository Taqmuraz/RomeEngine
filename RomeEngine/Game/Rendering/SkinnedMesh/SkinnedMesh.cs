using RomeEngine.IO;
using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class SkinnedMesh : IMesh, ISerializable
    {
        int[] indices;
        Matrix4x4[] bindPoses;
        MeshAttribute[] attributes;

        public Matrix4x4[] BindMatrices => bindPoses;

        public PolygonFormat PolygonFormat { get; private set; }
        public string[] JointNames { get; private set; }
        public static int MaxJointsSupported => 3;


        public SkinnedMesh()
        {
        }
        public SkinnedMesh
            (MeshAttribute[] attributes,
            int[] indices,
            string[] jointNames,
            Matrix4x4[] bindPoses,
            PolygonFormat polygonFormat) : this()
        {
            this.indices = indices;
            this.attributes = attributes;
            JointNames = jointNames;
            PolygonFormat = polygonFormat;
            this.bindPoses = bindPoses;
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
            yield return new GenericSerializableField<string[]>(nameof(JointNames), JointNames, value => JointNames = value, true);
            yield return new GenericSerializableField<int>(nameof(PolygonFormat), (int)PolygonFormat, value => PolygonFormat = (PolygonFormat)value, true);
            yield return new GenericSerializableField<Matrix4x4[]>(nameof(bindPoses), bindPoses, value => bindPoses = value, true);
        }

        public IVertexBuffer CreateVerticesAttributeBuffer(int attributeIndex)
        {
            return new VertexBuffer(attributes[attributeIndex].Type.ConvertFromAttributeBuffer(attributes[attributeIndex].Buffer));
        }
    }
}
