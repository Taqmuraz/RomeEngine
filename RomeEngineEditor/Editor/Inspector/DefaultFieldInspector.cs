using System;
using RomeEngine;

namespace RomeEngineEditor
{
    public sealed class DefaultFieldInspector : IFieldInspector
    {
        public bool CanInspect(Type type)
        {
            return true;
        }

        public void Inspect(string name, object value, Action<object> setter, Type type, EditorCanvas canvas, InspectorMenu inspectorMenu)
        {
            inspectorMenu.AllocateField(out Rect nameRect, out Rect valueRect);
            if (!string.IsNullOrEmpty(name)) canvas.DrawText(name, nameRect, inspectorMenu.NameTextOptions);
            canvas.DrawText(value == null ? "null" : value.ToString(), valueRect, inspectorMenu.ValueTextOptions);
        }
    }
}