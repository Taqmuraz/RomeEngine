using System;

namespace RomeEngine
{
    public sealed class VertexBuffer<TElement> : IVertexBuffer<TElement>
    {
        int position;
        TElement[] array;

        public VertexBuffer(int size)
        {
            array = new TElement[size];
            position = 0;
        }
        public VertexBuffer(TElement[] array)
        {
            this.array = array;
            position = 0;
        }

        public void Write(TElement value)
        {
            array[position++] = value;
        }

        public TElement[] ToArray()
        {
            return array;
        }
    }
    public sealed class VertexBuffer : IVertexBuffer
    {
        int position;
        Array buffer;

        public VertexBuffer(int size, MeshAttributeType type)
        {
            buffer = Array.CreateInstance(type.GetElementType(), size);
        }

        public void Write(object value)
        {
            buffer.SetValue(value, position++);
        }

        public Array ToArray()
        {
            return buffer;
        }
    }
}
