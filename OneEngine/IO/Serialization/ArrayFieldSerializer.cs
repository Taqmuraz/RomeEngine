using System;
using System.Linq;

namespace OneEngine.IO
{
    public sealed class ArrayFieldSerializer : CollectionFieldSerializer
    {
        public override bool CanSerializeType(Type type)
        {
            return typeof(Array).IsAssignableFrom(type);
        }

        protected override object CreateCollection(Type collectionType, int length)
        {
            return Array.CreateInstance(collectionType.GetElementType(), length);
        }

        protected override void ReadElement(object collection, int index, ISerializationStream stream)
        {
            var array = (Array)collection;
            int serializerIndex = stream.ReadInt();
            if (serializerIndex == -1) return;
            var serializer = Serializer.FieldSerializers[serializerIndex];
            array.SetValue(serializer.DeserializeField(collection.GetType().GetElementType(), stream), index);
        }

        protected override void WriteElement(object collection, int index, ISerializationStream stream)
        {
            var array = (Array)collection;
            var element = array.GetValue(index);
            if (element != null)
            {
                var serializer = Serializer.FieldSerializers.FirstOrDefault(s => s.CanSerializeType(element.GetType()));
                if (serializer != null)
                {
                    stream.WriteInt(Serializer.FieldSerializers.IndexOf(serializer));
                    serializer.SerializeField(element, stream);
                    return;
                }
            }
            stream.WriteInt(-1);
        }

        protected override int GetCollectionLength(object collection)
        {
            return ((Array)collection).Length;
        }
    }
}
