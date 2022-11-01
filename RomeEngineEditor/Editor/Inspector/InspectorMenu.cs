using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RomeEngine;
using RomeEngine.IO;

namespace RomeEngineEditor
{
    public sealed class InspectorMenu : EditorMenu
    {
        ISerializable InspectedObject { get; set; }
        IEnumerable<IFieldInspector> fieldInspectors;
        IEnumerable<IObjectInspector> objectInspectors;

        public InspectorMenu()
        {
            fieldInspectors = new IFieldInspector[]
            {
                new Vector2FieldInspector(),
                new Vector3FieldInspector(),
                new StringFieldInspector(),
                new FloatFieldInspector(),
                new IntFieldInspector(),
                new BoolFieldInspector(),
                new ColorFieldInspector(),
                new ListFieldInspector(),
                new ArrayFieldInspector(),
                new MeshFieldInspector(),
                new MaterialFieldInspector(),
                new SerializableFieldInspector(),
                new DefaultFieldInspector(),
            };
            objectInspectors = new IObjectInspector[]
            {
                new AnimatorInspector(),
                new GameObjectInspector(),
                new DefaultObjectInspector(),
            };
        }

        public void Inspect(ISerializable objectToInspect)
        {
            InspectedObject = objectToInspect;
        }

        public override void Draw(EditorCanvas canvas)
        {
            if (InspectedObject == null) return;

            fieldIndex = 0;

            canvas.DrawRect(Rect);
            
            try
            {
                GetObjectInspector(InspectedObject).Inspect(InspectedObject, this, canvas);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
            float diff = fieldIndex * elementHeight - Rect.Height;
            if (diff > 0f)
            {
                menuScrollbar = canvas.DrawScrollbar(GetHashCode(), 0f, diff, menuScrollbar, Rect.FromLocationAndSize(Rect.X - scrollbarWidth, Rect.Y, scrollbarWidth, Rect.Height), 1, Color32.gray);
            }
            else
            {
                menuScrollbar = 0f;
            }
        }

        public IFieldInspector GetFieldInspector(Type type)
        {
            return fieldInspectors.First(f => f.CanInspect(type));
        }
        public IObjectInspector GetObjectInspector(ISerializable inspectedObject)
        {
            return objectInspectors.First(o => o.CanInspect(inspectedObject));
        }

        public TextOptions ValueTextOptions { get; } = new TextOptions() { FontSize = 14f, Alignment = TextAlignment.MiddleLeft };
        public TextOptions NameTextOptions { get; } = new TextOptions() { FontSize = 14f, Alignment = TextAlignment.MiddleLeft };

        readonly float elementHeight = 30f;
        readonly float scrollbarWidth = 20f;
        float menuScrollbar = 0f;
        int fieldIndex;
        public Rect GetNextRect()
        {
            var result = GetCurrentRect();
            fieldIndex++;
            return result;
        }
        public Rect GetCurrentRect()
        {
            return new Rect(Rect.X, Rect.Y + elementHeight * fieldIndex - menuScrollbar, Rect.Width - scrollbarWidth, elementHeight);
        }
        public void GetCurrentField(out Rect name, out Rect value)
        {
            var rect = GetCurrentRect();
            rect.SplitHorizontal(out name, out value);
        }
        public void AllocateField(out Rect name, out Rect value)
        {
            var rect = GetNextRect();
            rect.SplitHorizontal(out name, out value);
        }
    }
}