using System;

namespace OneEngine.IO
{
    public sealed class StringFieldSerializer : IFieldSerializer
    {
        public bool CanSerializeType(Type type)
        {
            return type == typeof(string);
        }

        public void SerializeField(object value, ISerializationStream stream)
        {
            stream.WriteString((string)value);
        }

        public object DeserializeField(Type type, ISerializationStream stream)
        {
            return stream.ReadString();
        }
    }
}
