using System;

namespace OneEngine.IO
{
    public sealed class ObjectReferenceFieldSerializer : IFieldSerializer
    {
        public bool CanSerializeType(Type type)
        {
            throw new NotImplementedException();
        }

        public void SerializeField(object value, ISerializationStream stream)
        {
            throw new NotImplementedException();
        }

        public object DeserializeField(Type type, ISerializationStream stream)
        {
            throw new NotImplementedException();
        }
    }
}
