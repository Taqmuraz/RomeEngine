using System;
using System.Linq;
using System.Collections;

namespace RomeEngine.IO
{
    public sealed class DictionaryFieldSerializer : CollectionFieldSerializer
    {
        public override bool CanSerializeType(Type type)
        {
            return typeof(IDictionary).IsAssignableFrom(type);
        }

        protected override int GetCollectionLength(object collection)
        {
            return ((IDictionary)collection).Count;
        }

        protected override object CreateCollection(Type collectionType, int length, ISerializationContext context)
        {
            return collectionType.GetConstructors().First(c => c.GetParameters().Length == 0).Invoke(new object[0]);
        }

        protected override IFieldSerializer[] SerializersToRead(ISerializationContext context)
        {
            return new IFieldSerializer[]
            {
                Serializer.GetSerializerForType(context.Stream.ReadType()),
                Serializer.GetSerializerForType(context.Stream.ReadType())
            };
        }
        protected override IFieldSerializer[] SerializersToWrite(object collection, ISerializationContext context)
        {
            var type = collection.GetType();
            IFieldSerializer[] serializers = new IFieldSerializer[2];
            if (type.IsGenericType)
            {
                var args = collection.GetType().GetGenericArguments();
                context.Stream.WriteType(args[0]);
                context.Stream.WriteType(args[1]);
                serializers[0] = Serializer.GetSerializerForType(args[0]);
                serializers[1] = Serializer.GetSerializerForType(args[1]);
            }
            else
            {
                context.Stream.WriteType(typeof(object));
                context.Stream.WriteType(typeof(object));
                serializers[0] = serializers[1] = Serializer.GetSerializerForType(typeof(object));
            }
            return serializers;
        }

        protected override void ReadElement(object collection, int index, ISerializationContext context, IFieldSerializer[] serializers)
        {
            var dictionary = (IDictionary)collection;
            var key = serializers[0].DeserializeField(collection.GetType().GetGenericArguments()[0], context);
            object value = serializers[1].DeserializeField(collection.GetType().GetGenericArguments()[1], context);
            dictionary.Add(key, value);
        }

        protected override void WriteElement(object collection, int index, ISerializationContext context, IFieldSerializer[] serializers)
        {
            var dictionary = ((IDictionary)collection);
            var key = dictionary.Keys.Cast<object>().ToArray()[index];
            var serializer = serializers[0];
            serializer.SerializeField(key, context);

            var element = dictionary[key];
            serializers[1].SerializeField(element, context);
        }
    }
}
