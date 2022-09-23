namespace RomeEngine
{
    public interface IMeshAttribute<TVertex, TElement> : IMeshAttributeInfo where TVertex : IVertex
    {
        void WriteVertex(IVertexBuffer<TElement> buffer, TVertex vertex);
    }
}