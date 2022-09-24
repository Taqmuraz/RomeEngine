namespace RomeEngine
{
    public class VertexAttributeUV : IStaticMeshAttribute<float>
    {
        public int Size => 2;
        public MeshAttributeType Type => MeshAttributeType.Float;

        public void WriteVertex(IVertexBuffer<float> buffer, Vertex vertex)
        {
            buffer.Write(vertex.UV.x);
            buffer.Write(vertex.UV.y);
        }
    }
}
