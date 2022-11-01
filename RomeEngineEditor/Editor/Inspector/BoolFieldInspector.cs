using System;
using RomeEngine;

namespace RomeEngineEditor
{
    public sealed class BoolFieldInspector : IFieldInspector
    {
        public bool CanInspect(Type type)
        {
            return type == typeof(bool);
        }

        public void Inspect(string name, object value, Action<object> setter, Type type, EditorCanvas canvas, InspectorMenu inspectorMenu)
        {
            inspectorMenu.AllocateField(out Rect nameRect, out Rect valueRect);
            canvas.DrawText(name, nameRect, inspectorMenu.NameTextOptions);
            if (canvas.DrawButton(value.ToString(), valueRect, inspectorMenu.ValueTextOptions))
            {
                setter(!(bool)value);
            }
        }
    }
}