using System;
using RomeEngine;

namespace RomeEngineGame
{
    public sealed class Vector3FieldInspector : IFieldInspector
    {
        public bool CanInspect(Type type)
        {
            return type == typeof(Vector3);
        }

        public void Inspect(string name, object value, Action<object> setter, Type type, EditorCanvas canvas, InspectorMenu inspectorMenu)
        {
            inspectorMenu.AllocateField(out Rect nameRect, out Rect valueRect);
            canvas.DrawText(name, nameRect, inspectorMenu.NameTextOptions);

            Vector3 vector = (Vector3)value;

            if (canvas.DrawButton($"x:{vector.x}", Rect.FromLocationAndSize(valueRect.min, new Vector2(valueRect.Width * 0.33f, valueRect.Height)), inspectorMenu.ValueTextOptions))
            {
                EditorMenu.ShowMenu<StringInputMenu>(canvas, menu =>
                {
                    vector.x = menu.InputString.ToFloat();
                    setter(vector);
                }).WithHeader("x");
            }
            if (canvas.DrawButton($"y:{vector.y}", Rect.FromLocationAndSize(valueRect.min + new Vector2(valueRect.Width * 0.33f, 0f), new Vector2(valueRect.Width * 0.33f, valueRect.Height)), inspectorMenu.ValueTextOptions))
            {
                EditorMenu.ShowMenu<StringInputMenu>(canvas, menu =>
                {
                    vector.y = menu.InputString.ToFloat();
                    setter(vector);
                }).WithHeader("y");
            }
            if (canvas.DrawButton($"z:{vector.z}", Rect.FromLocationAndSize(valueRect.min + new Vector2(valueRect.Width * 0.66f, 0f), new Vector2(valueRect.Width * 0.33f, valueRect.Height)), inspectorMenu.ValueTextOptions))
            {
                EditorMenu.ShowMenu<StringInputMenu>(canvas, menu =>
                {
                    vector.z = menu.InputString.ToFloat();
                    setter(vector);
                }).WithHeader("z");
            }
        }
    }
}