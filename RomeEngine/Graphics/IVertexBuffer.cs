namespace RomeEngine
{
    public interface IVertexBuffer
    {
        void Write(float value);
        float[] ToArray();
    }
}
