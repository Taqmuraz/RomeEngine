namespace RomeEngine
{
    public class VertexVector3Attribute : IVertexAttribute
    {
        Vector3 vector;

        public VertexVector3Attribute(Vector3 vector)
        {
            this.vector = vector;
        }

        public float[] ToFloatsArray()
        {
            return new[] { vector.x, vector.y, vector.z };
        }

        public int Size => 3;
    }
}
