namespace RomeEngine
{
    public class VertexVector4Attribute : IVertexAttribute
    {
        Vector4 vector;

        public VertexVector4Attribute(Vector4 vector)
        {
            this.vector = vector;
        }

        public float[] ToFloatsArray()
        {
            return new[] { vector.x, vector.y, vector.z, vector.w };
        }

        public int Size => 4;
    }
}