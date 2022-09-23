namespace RomeEngine
{
    public class VertexAttributePosition : IMeshAttribute<Vertex>
    {
        public void WriteVertex(IVertexBuffer buffer, Vertex vertex)
        {
            Vector3 pos = vertex.Position;
            buffer.Write(pos.x);
            buffer.Write(pos.y);
            buffer.Write(pos.z);
        }

        public int Size => 3;
    }
}
