using RomeEngine;
using System;
using System.Linq;

namespace RomeEngineGame
{
    public sealed class MeshFieldInspector : IFieldInspector
    {
        public bool CanInspect(Type type)
        {
            return typeof(IMesh).IsAssignableFrom(type);
        }

        public void Inspect(string name, object value, Action<object> setter, Type type, EditorCanvas canvas, InspectorMenu inspectorMenu)
        {
            inspectorMenu.AllocateField(out Rect nameRect, out Rect valueRect);
            canvas.DrawText(name, nameRect, inspectorMenu.NameTextOptions);

            (string label, Func<IMesh> func)[] options = new (string, Func<IMesh>)[]
            {
                ("Cancel", () => (IMesh)value),
                ("Null", () => null),
                ("Box", () => StaticMesh.CreateBoxMesh()),
            };

            if (canvas.DrawButton("Change", valueRect, inspectorMenu.ValueTextOptions))
            {
                EditorMenu.ShowMenu<DropdownMenu>(canvas, menu => setter(options[menu.SelectedOption].func()))
                    .DropdownOptions = options.Select(o => o.label).ToArray();
            }
        }
    }
}