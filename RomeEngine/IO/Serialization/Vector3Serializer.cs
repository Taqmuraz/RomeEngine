using System;

namespace RomeEngine.IO
{
    public sealed class Vector3Serializer : IFieldSerializer
    {
        public bool CanSerializeType(Type type)
        {
            return type == typeof(Vector3);
        }

        public void SerializeField(object value, ISerializationContext context)
        {
            Vector3 vector = (Vector3)value;
            context.Stream.WriteFloat(vector.x);
            context.Stream.WriteFloat(vector.y);
            context.Stream.WriteFloat(vector.z);
        }

        public object DeserializeField(Type type, ISerializationContext context)
        {
            return new Vector3(context.Stream.ReadFloat(), context.Stream.ReadFloat(), context.Stream.ReadFloat());
        }
    }
}
