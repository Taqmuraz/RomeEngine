namespace RomeEngine
{
    public interface IVertexAttribute
    {
        int Size { get; }
        float[] ToFloatsArray();
    }
}