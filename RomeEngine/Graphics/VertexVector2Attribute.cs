namespace RomeEngine
{
    public class VertexVector2Attribute : IVertexAttribute
    {
        Vector2 vector;

        public VertexVector2Attribute(Vector2 vector)
        {
            this.vector = vector;
        }

        public float[] ToFloatsArray()
        {
            return new[] { vector.x, vector.y };
        }

        public int Size => 2;
    }
}
