using System;
using System.Linq;
using System.Collections;

namespace OneEngine.IO
{
    public sealed class ListFieldSerializer : CollectionFieldSerializer
    {
        public override bool CanSerializeType(Type type)
        {
            return typeof(IList).IsAssignableFrom(type);
        }

        protected override int GetCollectionLength(object collection)
        {
            return ((IList)collection).Count;
        }

        protected override object CreateCollection(Type collectionType, int length)
        {
            return collectionType.GetConstructors().First(c => c.GetParameters().Length == 0).Invoke(new object[0]);
        }

        protected override void ReadElement(object collection, int index, ISerializationStream stream)
        {
            var list = (IList)collection;
            int serializerIndex = stream.ReadInt();
            if (serializerIndex == -1) return;
            var serializer = Serializer.FieldSerializers[serializerIndex];
            list[index] = serializer.DeserializeField(collection.GetType().GetElementType(), stream);
        }

        protected override void WriteElement(object collection, int index, ISerializationStream stream)
        {
            var list = (IList)collection;
            var element = list[index];
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
    }
}
