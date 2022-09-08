using System;
using System.Collections;

namespace RomeEngine.IO
{
    public abstract class CollectionFieldSerializer : IFieldSerializer
    {
        public abstract bool CanSerializeType(Type type);

        protected abstract int GetCollectionLength(object collection);
        protected abstract object CreateCollection(Type collectionType, int length);
        protected abstract void ReadElement(object collection, int index, ISerializationContext context);
        protected abstract void WriteElement(object collection, int index, ISerializationContext context);

        public void SerializeField(object value, ISerializationContext context)
        {
            var collection = value as IEnumerable;
            
            if (collection == null)
            {
                context.Stream.WriteInt(-1);
            }
            else
            {
                int length = GetCollectionLength(collection);
                context.Stream.WriteInt(length);
                for (int i = 0; i < length; i++)
                {
                    WriteElement(collection, i, context);
                }
            }
        }

        public object DeserializeField(Type type, ISerializationContext context)
        {
            int length = context.Stream.ReadInt();
            if (length == -1) return null;
            object collection = CreateCollection(type, length);
            for (int i = 0; i < length; i++)
            {
                ReadElement(collection, i, context);
            }
            return collection;
        }
    }
}
