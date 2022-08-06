using System;
using OneEngine;

namespace OneEngineGame
{
    public sealed class ColorFieldInspector : IFieldInspector
    {
        public bool CanInspect(Type type)
        {
            return type == typeof(Color32);
        }

        public void Inspect(string name, object value, Action<object> setter, Type type, EditorCanvas canvas, InspectorMenu inspectorMenu)
        {
            inspectorMenu.AllocateField(out Rect nameRect, out Rect valueRect);
            canvas.DrawText(name, nameRect, inspectorMenu.NameTextOptions);
            var color = (Color32)value;
            if (canvas.DrawButton("Change color", valueRect, color.Negative, color, color, color, inspectorMenu.ValueTextOptions))
            {
                var colorMenu = EditorMenu.ShowMenu<ColorSelectMenu>(canvas, menu => setter(menu.Color));
                colorMenu.Color = color;
            }
        }
    }
}