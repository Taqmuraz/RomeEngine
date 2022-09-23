namespace RomeEngine
{
    public interface IMeshAttribute<TVertex> : IMeshAttributeInfo where TVertex : IVertex
    {
        void WriteVertex(IVertexBuffer buffer, TVertex vertex);
    }
}