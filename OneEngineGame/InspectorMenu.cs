using System.Collections;
using OneEngine;
using OneEngine.IO;

namespace OneEngineGame
{
    public sealed class InspectorMenu : EditorMenu
    {
        public ISerializable InspectedObject { get; set; }
        EditorCanvas canvas;
        IEnumerator routine;

        public InspectorMenu()
        {
            routine = DrawRoutine();
        }

        public override void Draw(EditorCanvas canvas)
        {
            this.canvas = canvas;
            if (routine != null) if (!routine.MoveNext()) routine = null;
        }
        IEnumerator DrawRoutine()
        {
            while (true)
            {
            START:
                if (InspectedObject == null)
                {
                    yield return null;
                    goto START;
                }
                yield return null;

                var inspectorRect = Rect;

                float elementWidth = inspectorRect.Width;
                float elementHeight = 30f;
                float offsetY = 0f;
                var valueTextOptions = new TextOptions() { FontSize = 14f, Alignment = TextAlignment.MiddleLeft };
                var nameTextOptions = new TextOptions() { FontSize = 14f, Alignment = TextAlignment.MiddleLeft };

                canvas.DrawRect(inspectorRect);

                void InspectField(string name, object value)
                {
                    if (!string.IsNullOrEmpty(name)) canvas.DrawText(name, new Rect(inspectorRect.X, inspectorRect.Y + offsetY, elementWidth * 0.5f, elementHeight), nameTextOptions);

                    var valueRect = new Rect(inspectorRect.X + elementWidth * 0.5f, inspectorRect.Y + offsetY, elementWidth * 0.5f, elementHeight);

                    if (value is ISerializable)
                    {
                        if (value == null)
                        {
                            canvas.DrawText("null", valueRect, valueTextOptions);
                        }
                        else if (canvas.DrawButton(value.ToString(), valueRect, valueTextOptions))
                        {
                            InspectedObject = (ISerializable)value;
                        }
                    }
                    else if (value is IList list)
                    {
                        canvas.DrawText($"Count:{list.Count}", valueRect, valueTextOptions);
                        foreach (var element in list)
                        {
                            offsetY += elementHeight;
                            InspectField(string.Empty, element);
                        }
                    }
                    else
                    {
                        canvas.DrawText(value == null ? "null" : value.ToString(), valueRect, valueTextOptions);
                    }
                    offsetY += elementHeight;
                }

                foreach (var field in InspectedObject.EnumerateFields())
                {
                    if (!field.HideInInspector) InspectField(field.Name, field.Value);
                }
                if (InspectedObject is GameObject gameObject)
                {
                    if (canvas.DrawButton("Add component", new Rect(inspectorRect.X, inspectorRect.Y + offsetY, elementWidth, elementHeight), new TextOptions() { FontSize = 25f, Alignment = TextAlignment.MiddleCenter }))
                    {
                        ShowMenu<StringInputMenu>(canvas, menu => gameObject.AddComponent(TypesMap.GetType(menu.InputString)));
                    }
                }
            }
        }
    }
}