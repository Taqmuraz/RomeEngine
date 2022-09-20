using System;
using RomeEngine;
using RomeEngine.IO;

namespace RomeEngineGame
{
    public sealed class SerializableFieldInspector : IFieldInspector
    {
        public bool CanInspect(Type type)
        {
            return typeof(ISerializable).IsAssignableFrom(type);
        }

        public void Inspect(string name, object value, Action<object> setter, Type type, EditorCanvas canvas, InspectorMenu inspectorMenu)
        {
            if (value == null)
            {
                inspectorMenu.AllocateField(out Rect nameRect, out Rect valueRect);
                if (!string.IsNullOrEmpty(name)) canvas.DrawText(name, nameRect, inspectorMenu.NameTextOptions);
                canvas.DrawText("null", valueRect, inspectorMenu.ValueTextOptions);
            }
            else
            {
                canvas.DrawRectWithText(type.Name, inspectorMenu.GetNextRect(), new TextOptions() { FontSize = 20f, Alignment = TextAlignment.MiddleCenter});
                var obj = (ISerializable)value;
                inspectorMenu.GetObjectInspector(obj).Inspect(obj, inspectorMenu, canvas);
            }
        }
    }
}