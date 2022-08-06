using System;
using OneEngine;
using OneEngine.IO;

namespace OneEngineGame
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
            if (value != null)
            {
                if (canvas.DrawButton(value.ToString(), valueRect, inspectorMenu.ValueTextOptions))
                {
                    inspectorMenu.Inspect((ISerializable)value);
                }
            }
        }
    }
}