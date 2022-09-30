using System;

namespace RomeEngine.IO
{
    public sealed class EmptySerializer : IFieldSerializer
    {
        public bool CanSerializeType(Type type) => true;

        public void SerializeField(object value, ISerializationContext context) { }

        public object DeserializeField(Type type, ISerializationContext context) => null;
    }
}
