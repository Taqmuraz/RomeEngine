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
        protected abstract TCollection AddElement(TCollection collection, TElement element);
        protected abstract TCollection RemoveElement(TCollection collection, int index);
        protected abstract TCollection CreateCollection(Type collectionType);
        protected abstract Type GetElementType(TCollection collection);
        protected abstract int GetCollectionLength(TCollection collection);

        public void Inspect(string name, object value, Action<object> setter, Type type, EditorCanvas canvas, InspectorMenu inspectorMenu)
        {
            TCollection collection = (TCollection)value;

            inspectorMenu.AllocateField(out Rect nameRect, out Rect valueRect);
            canvas.DrawText(name, nameRect, inspectorMenu.NameTextOptions);

            if (value != null)
            {
                canvas.DrawText($"length:{GetCollectionLength(collection)}", valueRect, inspectorMenu.ValueTextOptions);
                int index = 0;
                foreach (TElement element in EnumerateCollection(collection))
                {
                    int i = index;
                    var elementType = element == null ? typeof(object) : element.GetType();
                    inspectorMenu.GetCurrentField(out nameRect, out valueRect);
                    if (canvas.DrawButton("Remove", nameRect, inspectorMenu.NameTextOptions))
                    {
                        setter(collection = RemoveElement(collection, index));
                    }
                    inspectorMenu.GetFieldInspector(elementType).Inspect(string.Empty, element, e => SetElement(collection, i, (TElement)e), elementType, canvas, inspectorMenu);
                }
                inspectorMenu.AllocateField(out nameRect, out valueRect);
                if (canvas.DrawButton("Add element", valueRect, inspectorMenu.ValueTextOptions))
                {
                    var elementType = GetElementType(collection);
                    object instance = Activator.CreateInstance(elementType);
                    setter(collection = AddElement(collection, (TElement)instance));
                }
            }
            else
            {
                if (canvas.DrawButton("Create new", valueRect, inspectorMenu.ValueTextOptions))
                {
                    setter(CreateCollection(type));
                }
            }
        }
    }
}