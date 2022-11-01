using System;

namespace RomeEngine
{
    public sealed class VertexBuffer : IVertexBuffer
    {
        int position;
        Array buffer;

        public VertexBuffer(int size, MeshAttributeType type)
        {
            buffer = Array.CreateInstance(type.GetElementType(), size);
            ElementType = type;
        }

        public void Write(object value)
        {
            buffer.SetValue(value, position++);
        }

        public Array ToArray()
        {
            return buffer;
        }

        public MeshAttributeType ElementType { get; }
    }
}
