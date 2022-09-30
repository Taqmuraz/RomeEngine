using System;
using System.Collections;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ReadOnlyArrayFieldSerializer : CollectionFieldSerializer
    {
        public override bool CanSerializeType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ReadOnlyArray<>);
        }
        protected override object CreateCollection(Type collectionType, int length, ISerializationContext context)
        {
            return Array.CreateInstance(collectionType.GetGenericArguments().First(), length);
        }
        protected override IFieldSerializer[] SerializersToRead(ISerializationContext context)
        {
            return new IFieldSerializer[] { Serializer.GetSerializerForType(context.Stream.ReadType()) };
        }
        protected override IFieldSerializer[] SerializersToWrite(object collection, ISerializationContext context)
        {
            var type = collection.GetType().GetElementType();
            var serializer = Serializer.GetSerializerForType(type);
            context.Stream.WriteType(type);
            return new[] { serializer };
        }
        protected override int GetCollectionLength(object collection)
        {
            return ((IList)collection).Count;
        }
        protected override void ReadElement(object collection, int index, ISerializationContext context, IFieldSerializer[] serializers)
        {
            var array = (Array)collection;
            array.SetValue(serializers[0].DeserializeField(collection.GetType().GetElementType(), context), index);
        }
        protected override void WriteElement(object collection, int index, ISerializationContext context, IFieldSerializer[] serializers)
        {
            var array = (IList)collection;
            var element = array[index];
            serializers[0].SerializeField(element, context);
        }
    }
}
