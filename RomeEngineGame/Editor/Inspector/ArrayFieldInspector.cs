using System;
using System.Collections.Generic;

namespace OneEngineGame
{
    public sealed class ArrayFieldInspector : CollectionFieldInspector<Array, object>
    {
        protected override IEnumerable<object> EnumerateCollection(Array collection)
        {
            foreach (var element in collection) yield return element;
        }

        protected override Array SetElement(Array collection, int index, object element)
        {
            collection.SetValue(element, index);
            return collection;
        }

        protected override Array RemoveElement(Array collection, int index)
        {
            Array clone = Array.CreateInstance(collection.GetType().GetElementType(), collection.Length - 1);
            for (int i = 0, j = 0; i < collection.Length; i++)
            {
                if (i == index) continue;
                clone.SetValue(collection.GetValue(i), j);
                j++;
            }
            return clone;
        }
        protected override Array AddElement(Array collection, object element)
        {
            Array clone = Array.CreateInstance(collection.GetType().GetElementType(), collection.Length + 1);
            collection.CopyTo(clone, 0);
            clone.SetValue(element, collection.Length);
            return clone;
        }

        protected override Array CreateCollection(Type collectionType)
        {
            return Array.CreateInstance(collectionType.GetElementType(), 0);
        }

        protected override int GetCollectionLength(Array collection)
        {
            return collection.Length;
        }

        protected override Type GetElementType(Array collection)
        {
            return collection.GetType().GetElementType();
        }
    }
}