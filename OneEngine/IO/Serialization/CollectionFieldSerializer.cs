using System;
using System.Collections;

namespace OneEngine.IO
{
    public abstract class CollectionFieldSerializer : IFieldSerializer
    {
        public abstract bool CanSerializeType(Type type);

        protected abstract int GetCollectionLength(object collection);
        protected abstract object CreateCollection(Type collectionType, int length);
        protected abstract void ReadElement(object collection, int index, ISerializationStream stream);
        protected abstract void WriteElement(object collection, int index, ISerializationStream stream);

        public void SerializeField(object value, ISerializationStream stream)
        {
            var collection = value as IEnumerable;
            
            if (collection == null)
            {
                stream.WriteInt(-1);
            }
            else
            {
                int length = GetCollectionLength(collection);
                stream.WriteInt(length);
                for (int i = 0; i < length; i++)
                {
                    WriteElement(collection, i, stream);
                }
            }
        }

        public object DeserializeField(Type type, ISerializationStream stream)
        {
            int length = stream.ReadInt();
            if (length == -1) return null;
            object collection = CreateCollection(type, length);
            for (int i = 0; i < length; i++)
            {
                ReadElement(collection, i, stream);
            }
            return collection;
        }
    }
}
