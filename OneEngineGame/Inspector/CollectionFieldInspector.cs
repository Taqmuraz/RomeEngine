using System;
using System.Collections.Generic;
using OneEngine;

namespace OneEngineGame
{
    public abstract class CollectionFieldInspector<TCollection, TElement> : IFieldInspector
    {
        public virtual bool CanInspect(Type type)
        {
            return typeof(TCollection).IsAssignableFrom(type);
        }

        protected abstract IEnumerable<TElement> EnumerateCollection(TCollection collection);
        protected abstract TCollection SetElement(TCollection collection, int index, TElement element);
        protected abstract TCollection CreateCollection(int length, Type collectionType);
        protected abstract int GetCollectionLength(TCollection collection);

        public void Inspect(string name, object value, Action<object> setter, Type type, EditorCanvas canvas, InspectorMenu inspectorMenu)
        {
            TCollection collection = (TCollection)value;

            inspectorMenu.AllocateField(out Rect nameRect, out Rect valueRect);
            canvas.DrawText(name, nameRect, inspectorMenu.NameTextOptions);
            canvas.DrawText(collection == null ? "null" : $"length:{GetCollectionLength(collection)}", valueRect, inspectorMenu.ValueTextOptions);

            int index = 0;
            foreach (TElement element in EnumerateCollection(collection))
            {
                int i = index;
                var elementType = element == null ? typeof(object) : element.GetType();
                inspectorMenu.GetFieldInspector(elementType).Inspect(string.Empty, element, e => SetElement(collection, i, (TElement)e), elementType, canvas, inspectorMenu);
            }
        }
    }
}