using System;

namespace RomeEngine.IO
{
    public sealed class ArithmeticTypeFieldSerializer : IFieldSerializer
    {
        public bool CanSerializeType(Type type)
        {
            return type == typeof(int) || type == typeof(float);
        }

        public void SerializeField(object value, ISerializationContext context)
        {
            var type = value.GetType();
            if (type == typeof(int)) context.Stream.WriteInt((int)value);
            else context.Stream.WriteFloat((float)value);
        }

        public object DeserializeField(Type type, ISerializationContext context)
        {
            if (type == typeof(int)) return context.Stream.ReadInt();
            else return context.Stream.ReadFloat();
        }
    }
}
