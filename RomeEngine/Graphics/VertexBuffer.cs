using System;

namespace RomeEngine
{
    public sealed class VertexBuffer : IVertexBuffer
    {
        Array array;

        public VertexBuffer(Array array)
        {
            this.array = array;
        }

        public Array ToArray() => array;
    }
}
