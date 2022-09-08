using System;

namespace RomeEngine.IO
{
    public sealed class Color32Serializer : IFieldSerializer
    {
        public bool CanSerializeType(Type type)
        {
            return type == typeof(Color32);
        }

        public void SerializeField(object value, ISerializationContext context)
        {
            context.Stream.WriteInt(((Color32)value).Argb);
        }

        public object DeserializeField(Type type, ISerializationContext context)
        {
            return new Color32(context.Stream.ReadInt());
        }
    }
}
