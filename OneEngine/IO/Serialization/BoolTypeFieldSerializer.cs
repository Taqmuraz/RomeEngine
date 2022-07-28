using System;

namespace OneEngine.IO
{
    public sealed class BoolTypeFieldSerializer : IFieldSerializer
    {
        public bool CanSerializeType(Type type)
        {
            return type == typeof(bool);
        }

        public void SerializeField(object value, ISerializationContext context)
        {
            context.Stream.WriteInt((bool)value ? 1 : 0);
        }

        public object DeserializeField(Type type, ISerializationContext context)
        {
            return context.Stream.ReadInt() == 0 ? false : true;
        }
    }
}
