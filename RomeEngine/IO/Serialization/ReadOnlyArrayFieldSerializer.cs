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
        protected override object CreateCollection(Type collectionType, int length)
        {
            return Array.CreateInstance(collectionType.GetGenericArguments().First(), length);
        }
        protected override int GetCollectionLength(object collection)
        {
            return ((IList)collection).Count;
        }
        protected override void ReadElement(object collection, int index, ISerializationContext context)
        {
            var array = (Array)collection;
            string serializerIndex = context.Stream.ReadString();
            if (serializerIndex == Serializer.EmptySerializerKey)
            {
                return;
            }
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
            context.Stream.WriteInt(-1);
        }
    }
}
