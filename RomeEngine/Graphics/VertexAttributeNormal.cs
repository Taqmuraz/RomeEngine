namespace RomeEngine
{
    public class VertexAttributeNormal : IStaticMeshAttribute<float>
    {
        public void WriteVertex(IVertexBuffer<float> buffer, Vertex vertex)
        {
            Vector3 normal = vertex.Normal;
            buffer.Write(normal.x);
            buffer.Write(normal.y);
            buffer.Write(normal.z);
        }

        public int Size => 3;
        public MeshAttributeType Type => MeshAttributeType.Float;
    }
}
