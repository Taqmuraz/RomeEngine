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

        protected override object CreateCollection(Type collectionType, int length, ISerializationContext context)
        {
            var elementType = context.Stream.ReadType();
            return Array.CreateInstance(elementType, length);
        }
        protected override void ProcessCollection(ISerializationContext context, IEnumerable collection)
        {
            context.Stream.WriteType(collection.GetType().GetElementType());
        }

        protected override void ReadElement(object collection, int index, ISerializationContext context)
        {
            var array = (Array)collection;
            var serializerIndex = context.Stream.ReadString();
            if (serializerIndex == Serializer.EmptySerializerKey) return;
            var serializer = Serializer.FieldSerializers[serializerIndex];
            array.SetValue(serializer.DeserializeField(collection.GetType().GetElementType(), context), index);
        }

        protected override void WriteElement(object collection, int index, ISerializationContext context)
        {
            var array = (IList)collection;
            var element = array[index];
            if (element != null)
            {
                var serializer = Serializer.FieldSerializers.Values.FirstOrDefault(s => s.CanSerializeType(element.GetType()));
                if (serializer != null)
                {
                    context.Stream.WriteString(Serializer.GetSerializerKey(serializer));
                    serializer.SerializeField(element, context);
                    return;
                }
            }
            context.Stream.WriteString(Serializer.EmptySerializerKey);
        }

        protected override int GetCollectionLength(object collection)
        {
            return ((ICollection)collection).Count;
        }
    }
}
