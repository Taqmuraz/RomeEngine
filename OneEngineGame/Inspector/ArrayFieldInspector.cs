using System;
using System.Collections;
using System.Collections.Generic;

namespace OneEngineGame
{
    public sealed class ArrayFieldInspector : CollectionFieldInspector<IList, object>
    {
        protected override IEnumerable<object> EnumerateCollection(IList collection)
        {
            foreach (var element in collection) yield return element;
        }

        protected override IList SetElement(IList collection, int index, object element)
        {
            collection[index] = element;
            return collection;
        }

        protected override IList CreateCollection(int length, Type collectionType)
        {
            return Array.CreateInstance(collectionType.GetElementType(), length);
        }

        protected override int GetCollectionLength(IList collection)
        {
            return collection.Count;
        }
    }
}