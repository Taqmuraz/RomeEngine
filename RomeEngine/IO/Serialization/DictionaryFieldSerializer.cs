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

        protected override object CreateCollection(Type collectionType, int length)
        {
            return collectionType.GetConstructors().First(c => c.GetParameters().Length == 0).Invoke(new object[0]);
        }

        protected override void ReadElement(object collection, int index, ISerializationContext context)
        {
            var dictionary = (IDictionary)collection;
            int serializerIndex = context.Stream.ReadInt();
            if (serializerIndex == -1) return;
            var serializer = Serializer.FieldSerializers[serializerIndex];
            var key = serializer.DeserializeField(collection.GetType().GetElementType(), context);

            object value = null;
            serializerIndex = context.Stream.ReadInt();
            if (serializerIndex != -1)
            {
                serializer = Serializer.FieldSerializers[serializerIndex];
                value = serializer.DeserializeField(collection.GetType().GetElementType(), context);
            }
            dictionary.Add(key, value);
        }

        protected override void WriteElement(object collection, int index, ISerializationContext context)
        {
            var dictionary = ((IDictionary)collection);
            var key = dictionary.Keys.Cast<object>().ToArray()[index];
            var serializer = Serializer.FieldSerializers.FirstOrDefault(s => s.CanSerializeType(key.GetType()));
            if (serializer == null) return;
            context.Stream.WriteInt(Serializer.FieldSerializers.IndexOf(serializer));
            serializer.SerializeField(key, context);

            var element = dictionary[key];
            if (element != null)
            {
                serializer = Serializer.FieldSerializers.FirstOrDefault(s => s.CanSerializeType(element.GetType()));
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
