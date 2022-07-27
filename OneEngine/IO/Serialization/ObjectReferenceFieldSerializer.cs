using System;

namespace OneEngine.IO
{
    public sealed class ObjectReferenceFieldSerializer : IFieldSerializer
    {
        public bool CanSerializeType(Type type)
        {
            return typeof(ISerializable).IsAssignableFrom(type);
        }

        public void SerializeField(object value, ISerializationContext context)
        {
            var obj = value as ISerializable;
            if (obj == null) context.Stream.WriteInt(-1);
            else context.Stream.WriteInt(context.Objects.IndexOf(obj));
        }

        public object DeserializeField(Type type, ISerializationContext context)
        {
            int index = context.Stream.ReadInt();
            return index == -1 ? null : context.Objects[index];
        }
    }
}
