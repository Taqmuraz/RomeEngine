using System;
using System.Collections;
using System.Collections.Generic;

namespace RomeEngine.IO
{
    public abstract class CollectionFieldSerializer : IFieldSerializer
    {
        public abstract bool CanSerializeType(Type type);

        protected abstract int GetCollectionLength(object collection);
        protected abstract object CreateCollection(Type collectionType, int length, ISerializationContext context);
        protected abstract void ReadElement(object collection, int index, ISerializationContext context, IFieldSerializer[] serializers);
        protected abstract void WriteElement(object collection, int index, ISerializationContext context, IFieldSerializer[] serializers);

        protected abstract IFieldSerializer[] SerializersToRead(ISerializationContext context);
        protected abstract IFieldSerializer[] SerializersToWrite(object collection, ISerializationContext context);

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
                var serializers = SerializersToWrite(value, context);
                WriteCollectionMeta(value, context);
                for (int i = 0; i < length; i++)
                {
                    WriteElement(collection, i, context, serializers);
                }
            }
        }

        protected virtual void WriteCollectionMeta(object collection, ISerializationContext context) { }

        public object DeserializeField(Type type, ISerializationContext context)
        {
            int length = context.Stream.ReadInt();
            if (length == -1) return null;
            var serializers = SerializersToRead(context);
            object collection = CreateCollection(type, length, context);
            for (int i = 0; i < length; i++)
            {
                ReadElement(collection, i, context, serializers);
            }
            return collection;
        }
    }
}
