using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaRawMesh : ColladaStackContainingObject<ColladaVertexBuffer>
    {
        string id;

        public ColladaRawMesh(string id)
        {
            this.id = id;
        }
        ColladaVertexBuffer CurrentBuffer => CurrentElement;

        public override string ToString() => id;
        public string Id => id;

        public ColladaStackContainingObject<TrianglesData> TrianglesData { get; } = new ColladaStackContainingObject<TrianglesData>();

        public void WriteBuffer(string value)
        {
            CurrentBuffer.Buffer = value;
        }

        public void WriteAttribute(ColladaVertexAttribute attribute)
        {
            CurrentBuffer.Attribute = attribute;
        }
        public void WriteAttributeProperty(string name, string type)
        {
            CurrentBuffer.Attribute.AddProperty(name, type);
        }

        static char[] separators = new char[] { ' ' };
        static T[] ReadBuffer<T>(string buffer, Func<string, T> parseFunc)
        {
            return buffer.Split(separators, StringSplitOptions.RemoveEmptyEntries).Select(v => parseFunc(v)).ToArray();
        }
        static Array ReadBuffer(string buffer, string bufferType)
        {
            bufferType = bufferType.ToLower();
            switch (bufferType)
            {
                case "float":return ReadBuffer<float>(buffer, v => v.ToFloat());
                case "int":return ReadBuffer<int>(buffer, v => int.Parse(v));
                default:
                    throw new System.InvalidOperationException($"Unsupported buffer type : {bufferType}");
            }
        }

        public int SubmeshesCount => TrianglesData.Elements.Count;

        public SkinnedMesh BuildMesh(int submeshIndex)
        {
            var buffers = Elements.ToArray();
            int buffersCount = buffers.Length;

            Array[] bufferArrays = new Array[buffersCount];
            Array[] newBufferArrays = new Array[buffersCount];

            int[] indicesBuffer = ReadBuffer(TrianglesData.Elements[submeshIndex].Indices, v => int.Parse(v));
            int newIndicesCount = indicesBuffer.Length / buffersCount;
            int[] newIndices = new int[newIndicesCount];

            for (int i = 0; i < buffersCount; i++)
            {
                bufferArrays[i] = ReadBuffer(buffers[i].Buffer, buffers[i].Attribute.Properties.First().Type);
                newBufferArrays[i] = Array.CreateInstance(bufferArrays[i].GetType().GetElementType(), buffers[i].Attribute.Size * newIndicesCount);
            }

            for (int i = 0; i < newIndicesCount; i++)
            {
                for (int attribute = 0; attribute < buffersCount; attribute++)
                {
                    int index = indicesBuffer[i * buffersCount + attribute];
                    int size = buffers[attribute].Attribute.Size;
                    for (int element = 0; element < size; element++) newBufferArrays[attribute].SetValue(bufferArrays[attribute].GetValue(index * size + element), i * size + element);
                }
                newIndices[i] = i;
            }
            var namedBuffers = Enumerable.Range(0, buffersCount).Select(i => (name: buffers[i].Id.Replace($"{id}-", string.Empty), buffer: newBufferArrays[i])).ToDictionary(b => b.name, b => b.buffer);

            return new SkinnedMesh
                (
                (float[])namedBuffers["positions"],
                (float[])namedBuffers["map-0"],
                (float[])namedBuffers["normals"],
                (int[])newIndices
                );
        }
    }
}
