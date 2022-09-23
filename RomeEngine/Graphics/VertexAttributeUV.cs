namespace RomeEngine
{
    public class VertexAttributeUV : IMeshAttribute<Vertex>
    {
        public int Size => 2;

        public void WriteVertex(IVertexBuffer buffer, Vertex vertex)
        {
            buffer.Write(vertex.UV.x);
            buffer.Write(vertex.UV.y);
        }
    }
}
