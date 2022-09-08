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
            int serializerIndex = context.Stream.ReadInt();
            if (serializerIndex == -1)
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
                var serializer = Serializer.FieldSerializers.FirstOrDefault(s => s.CanSerializeType(element.GetType()));
                if (serializer != null)
                {
                    context.Stream.WriteInt(Serializer.FieldSerializers.IndexOf(serializer));
                    serializer.SerializeField(element, context);
                    return;
                }
            }
            context.Stream.WriteInt(-1);
        }
    }
}
