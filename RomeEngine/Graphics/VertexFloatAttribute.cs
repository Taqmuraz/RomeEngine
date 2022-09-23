namespace RomeEngine
{
    public class VertexFloatAttribute : IVertexAttribute
    {
        float value;

        public VertexFloatAttribute(float value)
        {
            this.value = value;
        }

        public float[] ToFloatsArray()
        {
            return new[] { value };
        }

        public int Size => 1;
    }
}
