using System;

namespace OneEngine.IO
{
    public sealed class Vector2Serializer : IFieldSerializer
    {
        public bool CanSerializeType(Type type)
        {
            return type == typeof(Vector2);
        }

        public void SerializeField(object value, ISerializationContext context)
        {
            Vector2 vector = (Vector2)value;
            context.Stream.WriteFloat(vector.x);
            context.Stream.WriteFloat(vector.y);
        }

        public object DeserializeField(Type type, ISerializationContext context)
        {
            return new Vector2(context.Stream.ReadFloat(), context.Stream.ReadFloat());
        }
    }
}
