using System;

namespace RomeEngine.IO
{
    public sealed class StringFieldSerializer : IFieldSerializer
    {
        public bool CanSerializeType(Type type)
        {
            return type == typeof(string);
        }

        public void SerializeField(object value, ISerializationContext context)
        {
            context.Stream.WriteString((string)value);
        }

        public object DeserializeField(Type type, ISerializationContext context)
        {
            return context.Stream.ReadString();
        }
    }
}
