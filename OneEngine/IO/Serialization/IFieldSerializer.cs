using System;

namespace OneEngine.IO
{
    public interface IFieldSerializer
    {
        bool CanSerializeType(Type type);
        void SerializeField(object value, ISerializationStream stream);
        object DeserializeField(Type type, ISerializationStream stream);
    }
}
