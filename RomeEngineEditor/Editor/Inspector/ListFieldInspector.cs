using System;
using System.Collections;
using System.Collections.Generic;

namespace RomeEngineEditor
{
    public sealed class ListFieldInspector : CollectionFieldInspector<IList, object>
    {
        public override bool CanInspect(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }

        protected override IEnumerable<object> EnumerateCollection(IList collection)
        {
            foreach (var element in collection) yield return element;
        }
        protected override IList SetElement(IList collection, int index, object element)
        {
            collection[index] = element;
            return collection;
        }
        protected override IList AddElement(IList collection, object element)
        {
            collection.Add(element);
            return collection;
        }
        protected override IList CreateCollection(Type collectionType)
        {
            return (IList)Activator.CreateInstance(collectionType);
        }
        protected override int GetCollectionLength(IList collection)
        {
            return collection.Count;
        }
        protected override Type GetElementType(IList collection)
        {
            return collection.GetType().GetGenericArguments()[0];
        }
        protected override IList RemoveElement(IList collection, int index)
        {
            collection.RemoveAt(index);
            return collection;
        }
    }
}