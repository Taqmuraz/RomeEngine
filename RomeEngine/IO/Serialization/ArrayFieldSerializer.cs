﻿using System;
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

        protected override object CreateCollection(Type collectionType, int length)
        {
            return Array.CreateInstance(collectionType.GetElementType(), length);
        }

        protected override void ReadElement(object collection, int index, ISerializationContext context)
        {
            var array = (Array)collection;
            int serializerIndex = context.Stream.ReadInt();
            if (serializerIndex == -1) return;
            var serializer = Serializer.FieldSerializers[serializerIndex];
            array.SetValue(serializer.DeserializeField(collection.GetType().GetElementType(), context), index);
        }

        protected override void WriteElement(object collection, int index, ISerializationContext context)
        {
            var array = (IList)collection;
            var element = array[index];
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

        protected override int GetCollectionLength(object collection)
        {
            return ((ICollection)collection).Count;
        }
    }
}
