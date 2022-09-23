namespace RomeEngine
{
    public interface IVertexBuffer<TElement>
    {
        void Write(TElement value);
        TElement[] ToArray();
    }
}
