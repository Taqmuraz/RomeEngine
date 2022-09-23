namespace RomeEngine
{
    public class VertexAttributeNormal : IMeshAttribute<Vertex>
    {
        public void WriteVertex(IVertexBuffer buffer, Vertex vertex)
        {
            Vector3 normal = vertex.Normal;
            buffer.Write(normal.x);
            buffer.Write(normal.y);
            buffer.Write(normal.z);
        }

        public int Size => 3;
    }
}
