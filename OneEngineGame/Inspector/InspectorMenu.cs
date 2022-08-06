using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OneEngine;
using OneEngine.IO;

namespace OneEngineGame
{
    public sealed class InspectorMenu : EditorMenu
    {
        ISerializable InspectedObject { get; set; }
        IEnumerable<IFieldInspector> fieldInspectors;
        IEnumerable<IObjectInspector> objectInspectors;
        EditorCanvas canvas;
        IEnumerator routine;

        public InspectorMenu()
        {
            fieldInspectors = new IFieldInspector[]
            {
                new Vector2FieldInspector(),
                new StringFieldInspector(),
                new FloatFieldInspector(),
                new IntFieldInspector(),
                new BoolFieldInspector(),
                new ColorFieldInspector(),
                new ListFieldInspector(),
                new ArrayFieldInspector(),
                new SerializableFieldInspector(),
                new DefaultFieldInspector(),
            };
            objectInspectors = new IObjectInspector[]
            {
                new GameObjectInspector(),
                new DefaultObjectInspector(),
            };
            routine = DrawRoutine();
        }

        public void Inspect(ISerializable objectToInspect)
        {
            InspectedObject = objectToInspect;
        }

        public override void Draw(EditorCanvas canvas)
        {
            this.canvas = canvas;
            if (routine != null) if (!routine.MoveNext()) routine = null;
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
        int fieldIndex;
        public Rect GetNextRect()
        {
            var result = GetCurrentRect();
            fieldIndex++;
            return result;
        }
        public Rect GetCurrentRect()
        {
            return new Rect(Rect.X, Rect.Y + elementHeight * fieldIndex, Rect.Width, elementHeight);
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
            }
        }
    }
}