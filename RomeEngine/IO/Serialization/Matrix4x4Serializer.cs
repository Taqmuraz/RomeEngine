using System;

namespace RomeEngine.IO
{
    public sealed class Matrix4x4Serializer : IFieldSerializer
    {
        public bool CanSerializeType(Type type)
        {
            return type == typeof(Matrix4x4);
        }

        float[] matrixBuffer = new float[16];

        public void SerializeField(object value, ISerializationContext context)
        {
            var matrix = (Matrix4x4)value;
            matrix.ToFloatArray(matrixBuffer);
            for (int i = 0; i < matrixBuffer.Length; i++) context.Stream.WriteFloat(matrixBuffer[i]);
        }

        public object DeserializeField(Type type, ISerializationContext context)
        {
            for (int i = 0; i < matrixBuffer.Length; i++)
            {
                matrixBuffer[i] = context.Stream.ReadFloat();
            }
            return Matrix4x4.FromFloatsArray(matrixBuffer);
        }
    }
}
