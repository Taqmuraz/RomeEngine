using System;
using RomeEngine;
using RomeEngine.IO;

namespace RomeEngineEditor
{
    public sealed class SerializableFieldInspector : IFieldInspector
    {
        public bool CanInspect(Type type)
        {
            return typeof(ISerializable).IsAssignableFrom(type);
        }

        public void Inspect(string name, object value, Action<object> setter, Type type, EditorCanvas canvas, InspectorMenu inspectorMenu)
        {
            inspectorMenu.AllocateField(out Rect nameRect, out Rect valueRect);
            if (!string.IsNullOrEmpty(name)) canvas.DrawText(name, nameRect, inspectorMenu.NameTextOptions);

            if (value == null)
            {
                canvas.DrawText("null", valueRect, inspectorMenu.ValueTextOptions);
            }
            else
            {
                if (canvas.DrawButton("Inspect", valueRect, inspectorMenu.ValueTextOptions))
                {
                    inspectorMenu.Inspect((ISerializable)value);
                }
            }
        }
    }
}