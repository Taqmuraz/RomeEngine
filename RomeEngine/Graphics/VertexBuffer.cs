using System;

namespace RomeEngine
{
    public sealed class VertexBuffer<TElement> : Buffer<TElement>, IVertexBuffer<TElement>
    {
        public VertexBuffer(TElement[] array) : base(array)
        {
        }

        public VertexBuffer(int size) : base(size)
        {
        }
    }
}
