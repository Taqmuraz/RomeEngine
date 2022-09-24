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
}
