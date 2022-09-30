using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ArrayFieldSerializer : CollectionFieldSerializer
    {
        public override bool CanSerializeType(Type type)
        {
            return typeof(Array).IsAssignableFrom(type);
        }

        protected override IFieldSerializer[] SerializersToRead(ISerializationContext context)
        {
            return new IFieldSerializer[] { Serializer.GetSerializerForType(context.Stream.ReadType()) };
        }
        protected override IFieldSerializer[] SerializersToWrite(object collection, ISerializationContext context)
        {
            var type = collection.GetType().GetElementType();
            IFieldSerializer serialzier = Serializer.GetSerializerForType(type);
            context.Stream.WriteType(type);
            return new IFieldSerializer[] { serialzier };
        }

        protected override object CreateCollection(Type collectionType, int length, ISerializationContext context)
        {
            return Array.CreateInstance(context.Stream.ReadType(), length);
        }
        protected override void WriteCollectionMeta(object collection, ISerializationContext context)
        {
            context.Stream.WriteType(collection.GetType().GetElementType());
        }

        protected override void ReadElement(object collection, int index, ISerializationContext context, IFieldSerializer[] serializers)
        {
            var array = (Array)collection;
            array.SetValue(serializers[0].DeserializeField(collection.GetType().GetElementType(), context), index);
        }

        protected override void WriteElement(object collection, int index, ISerializationContext context, IFieldSerializer[] serialzers)
        {
            var array = (IList)collection;
            var element = array[index];
            serialzers[0].SerializeField(element, context);
        }

        protected override int GetCollectionLength(object collection)
        {
            return ((ICollection)collection).Count;
        }
    }
}
