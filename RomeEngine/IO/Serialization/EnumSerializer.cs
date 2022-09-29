using System;

namespace RomeEngine.IO
{
    public sealed class EnumSerializer : IFieldSerializer
    {
        public bool CanSerializeType(Type type)
        {
            return type.IsEnum;
        }

        public void SerializeField(object value, ISerializationContext context)
        {
            context.Stream.WriteInt((int)value);
        }

        public object DeserializeField(Type type, ISerializationContext context)
        {
            return context.Stream.ReadInt();
        }
    }
}
