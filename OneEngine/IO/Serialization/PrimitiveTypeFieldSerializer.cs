using System;

namespace OneEngine.IO
{
    public sealed class PrimitiveTypeFieldSerializer : IFieldSerializer
    {
        public bool CanSerializeType(Type type)
        {
            return type == typeof(int) || type == typeof(float);
        }

        public void SerializeField(object value, ISerializationStream stream)
        {
            var type = value.GetType();
            if (type == typeof(int)) stream.WriteInt((int)value);
            else stream.WriteFloat((float)value);
        }

        public object DeserializeField(Type type, ISerializationStream stream)
        {
            if (type == typeof(int)) return stream.ReadInt();
            else return stream.ReadFloat();
        }
    }
}
