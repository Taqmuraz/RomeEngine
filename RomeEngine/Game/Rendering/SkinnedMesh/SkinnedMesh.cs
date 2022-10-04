using RomeEngine.IO;
using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class SkinnedMesh : IMesh, ISourceObject
    {
        ISerializable ISourceObject.CloneSourceReference() => this;

        float[] vertices;
        float[] texcoords;
        float[] normals;
        int[] indices;
        float[] weights;
        int[] joints;
        Matrix4x4[] bindPoses;

        public Matrix4x4[] BindMatrices => bindPoses;

        public PolygonFormat PolygonFormat { get; private set; }
        public string[] JointNames { get; private set; }
        public Array[] Buffers => new Array[] { vertices, texcoords, normals, weights, joints };

        public static int MaxJointsSupported => 3;

        ReadOnlyArray<IMeshAttributeInfo> attributes;

        public SkinnedMesh()
        {
            attributes = new IMeshAttributeInfo[]
            {
                new CustomMeshAttribute(3, MeshAttributeType.Float),
                new CustomMeshAttribute(2, MeshAttributeType.Float),
                new CustomMeshAttribute(3, MeshAttributeType.Float),
                new CustomMeshAttribute(MaxJointsSupported, MeshAttributeType.Float),
                new CustomMeshAttribute(MaxJointsSupported, MeshAttributeType.Int),
            };
        }
        public SkinnedMesh
            (float[] vertices,
            float[] texcoords,
            float[] normals,
            float[] weights,
            int[] joints,
            int[] indices,
            string[] jointNames,
            Matrix4x4[] bindPoses,
            PolygonFormat polygonFormat) : this()
        {
            this.vertices = vertices;
            this.texcoords = texcoords;
            this.normals = normals;
            this.indices = indices;
            this.weights = weights;
            this.joints = joints;
            JointNames = jointNames;
            PolygonFormat = polygonFormat;
            this.bindPoses = bindPoses;
        }

        public IEnumerable<int> EnumerateIndices()
        {
            return indices;
        }

        public ReadOnlyArray<IMeshAttributeInfo> Attributes => attributes;

        IVertexBuffer<T> CreateBuffer<T>(int bufferIndex)
        {
            return new VertexBuffer<T>((T[])Buffers[bufferIndex]);
        }

        public IVertexBuffer<float> CreateVerticesFloatAttributeBuffer(int attributeIndex)
        {
            return CreateBuffer<float>(attributeIndex);
        }

        public IVertexBuffer<int> CreateVerticesIntAttributeBuffer(int attributeIndex)
        {
            return CreateBuffer<int>(attributeIndex);
        }

        public int PositionAttributeIndex => 0;
        public int TexcoordAttributeIndex => 1;
        public int NormalAttributeIndex => 2;

        public IEnumerable<SerializableField> EnumerateFields()
        {
            yield return new GenericSerializableField<float[]>(nameof(vertices), vertices, value => vertices = value, true);
            yield return new GenericSerializableField<float[]>(nameof(normals), normals, value => normals = value, true);
            yield return new GenericSerializableField<float[]>(nameof(texcoords), texcoords, value => texcoords = value, true);
            yield return new GenericSerializableField<int[]>(nameof(indices), indices, value => indices = value, true);
            yield return new GenericSerializableField<float[]>(nameof(weights), weights, value => weights = value, true);
            yield return new GenericSerializableField<int[]>(nameof(joints), joints, value => joints = value, true);
            yield return new GenericSerializableField<string[]>(nameof(JointNames), JointNames, value => JointNames = value, true);
            yield return new GenericSerializableField<int>(nameof(PolygonFormat), (int)PolygonFormat, value => PolygonFormat = (PolygonFormat)value, true);
            yield return new GenericSerializableField<Matrix4x4[]>(nameof(bindPoses), bindPoses, value => bindPoses = value, true);
        }
    }
}
