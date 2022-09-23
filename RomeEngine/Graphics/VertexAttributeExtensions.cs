namespace RomeEngine
{
    public static class VertexAttributeExtensions
    {
        public static Vector3 ReadVector3(this IVertexAttribute attribute)
        {
            var array = attribute.ToFloatsArray();
            var vector = new Vector3();
            int length = System.Math.Min(3, array.Length);
            for (int i = 0; i < length; i++) vector[i] = array[i];
            return vector;
        }
        public static Vector2 ReadVector2(this IVertexAttribute attribute)
        {
            var array = attribute.ToFloatsArray();
            var vector = new Vector2();
            int length = System.Math.Min(2, array.Length);
            for (int i = 0; i < length; i++) vector[i] = array[i];
            return vector;
        }
        public static Vector4 ReadVector4(this IVertexAttribute attribute)
        {
            var array = attribute.ToFloatsArray();
            var vector = new Vector4();
            int length = System.Math.Min(4, array.Length);
            for (int i = 0; i < length; i++) vector[i] = array[i];
            return vector;
        }
    }
}