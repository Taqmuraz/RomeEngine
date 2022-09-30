using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace RomeEngine.IO
{
    public sealed class ListFieldSerializer : CollectionFieldSerializer
    {
        public override bool CanSerializeType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        protected override int GetCollectionLength(object collection)
        {
            return ((IList)collection).Count;
        }

        protected override object CreateCollection(Type collectionType, int length, ISerializationContext context)
        {
            return collectionType.GetConstructors().First(c => c.GetParameters().Length == 0).Invoke(new object[0]);
        }

        protected override IFieldSerializer[] SerializersToRead(ISerializationContext context)
        {
            return new IFieldSerializer[] { Serializer.GetSerializerForType(context.Stream.ReadType()) };
        }
        protected override IFieldSerializer[] SerializersToWrite(object collection, ISerializationContext context)
        {
            var type = collection.GetType().GetGenericArguments()[0];
            context.Stream.WriteType(type);
            return new[] { Serializer.GetSerializerForType(type) };
        }

        protected override void ReadElement(object collection, int index, ISerializationContext context, IFieldSerializer[] serializers)
        {
            var list = (IList)collection;
            list.Add(serializers[0].DeserializeField(collection.GetType().GetElementType(), context));
        }

        protected override void WriteElement(object collection, int index, ISerializationContext context, IFieldSerializer[] serializers)
        {
            var list = (IList)collection;
            var element = list[index];
            serializers[0].SerializeField(element, context);
        }
    }
}
