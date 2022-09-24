namespace RomeEngine
{
    public class VertexAttributePosition : IStaticMeshAttribute<float>
    {
        public void WriteVertex(IVertexBuffer<float> buffer, Vertex vertex)
        {
            Vector3 pos = vertex.Position;
            buffer.Write(pos.x);
            buffer.Write(pos.y);
            buffer.Write(pos.z);
        }

        public int Size => 3;
        public MeshAttributeType Type => MeshAttributeType.Float;
    }
}
