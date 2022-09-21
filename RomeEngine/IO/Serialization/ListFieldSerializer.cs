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

        protected override object CreateCollection(Type collectionType, int length)
        {
            return collectionType.GetConstructors().First(c => c.GetParameters().Length == 0).Invoke(new object[0]);
        }

        protected override void ReadElement(object collection, int index, ISerializationContext context)
        {
            var list = (IList)collection;
            string serializerIndex = context.Stream.ReadString();
            if (serializerIndex == Serializer.EmptySerializerKey)
            {
                list.Add(default);
                return;
            }
            var serializer = Serializer.FieldSerializers[serializerIndex];
            list.Add(serializer.DeserializeField(collection.GetType().GetElementType(), context));
        }

        protected override void WriteElement(object collection, int index, ISerializationContext context)
        {
            var list = (IList)collection;
            var element = list[index];
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
    }
}
