namespace RomeEngine
{
    public sealed class VertexBuffer : IVertexBuffer
    {
        int position;
        float[] array;

        public VertexBuffer(int size)
        {
            array = new float[size];
            position = 0;
        }

        public void Write(float value)
        {
            array[position++] = value;
        }

        public float[] ToArray()
        {
            return array;
        }
    }
}
