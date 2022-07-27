using System;
using System.Linq;
using System.Collections;

namespace OneEngine.IO
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

        protected override void ReadElement(object collection, int index, ISerializationStream stream)
        {
            var dictionary = (IDictionary)collection;
            int serializerIndex = stream.ReadInt();
            if (serializerIndex == -1) return;
            var serializer = Serializer.FieldSerializers[serializerIndex];
            var key = serializer.DeserializeField(collection.GetType().GetElementType(), stream);

            object value = null;
            serializerIndex = stream.ReadInt();
            if (serializerIndex != -1)
            {
                serializer = Serializer.FieldSerializers[serializerIndex];
                value = serializer.DeserializeField(collection.GetType().GetElementType(), stream);
            }
            dictionary.Add(key, value);
        }

        protected override void WriteElement(object collection, int index, ISerializationStream stream)
        {
            var dictionary = ((IDictionary)collection);
            var key = dictionary.Keys.Cast<object>().ToArray()[index];
            var serializer = Serializer.FieldSerializers.FirstOrDefault(s => s.CanSerializeType(key.GetType()));
            if (serializer == null) return;
            stream.WriteInt(Serializer.FieldSerializers.IndexOf(serializer));
            serializer.SerializeField(key, stream);

            var element = dictionary[key];
            if (element != null)
            {
                serializer = Serializer.FieldSerializers.FirstOrDefault(s => s.CanSerializeType(element.GetType()));
                if (serializer != null)
                {
                    stream.WriteInt(Serializer.FieldSerializers.IndexOf(serializer));
                    serializer.SerializeField(element, stream);
                    return;
                }
            }
            stream.WriteInt(-1);
        }
    }
}
