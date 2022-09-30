namespace RomeEngine
{
    public interface IStaticMeshAttribute<TElement> : IMeshAttribute<Vertex, TElement>
    {
        void WriteVertex(IVertexBuffer<TElement> buffer, Vertex vertex);
    }
}