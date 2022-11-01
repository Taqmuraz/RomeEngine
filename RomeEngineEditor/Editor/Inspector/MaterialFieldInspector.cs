using RomeEngine;
using System;
using System.Linq;

namespace RomeEngineEditor
{
    public sealed class MaterialFieldInspector : IFieldInspector
    {
        public bool CanInspect(Type type)
        {
            return typeof(Material).IsAssignableFrom(type);
        }

        public void Inspect(string name, object value, Action<object> setter, Type type, EditorCanvas canvas, InspectorMenu inspectorMenu)
        {
            inspectorMenu.AllocateField(out var nameRect, out var valueRect);
            canvas.DrawText(name, nameRect, inspectorMenu.NameTextOptions);

            (string label, Action action)[] materialOptions = new (string, Action)[]
            {
                ("Cancel", () => { }),
                ("New single texture", () =>
                {
                    Engine.Instance.Runtime.ShowFileOpenDialog("./", "Select texture", file => setter(new SingleTextureMaterial("New material") { TextureFileName = Engine.Instance.Runtime.FileSystem.RelativePath(file) }));
                }),
            };

            if (canvas.DrawButton("Change", valueRect, inspectorMenu.ValueTextOptions))
            {
                EditorMenu.ShowMenu<DropdownMenu>(canvas, menu => materialOptions[menu.SelectedOption].action())
                    .DropdownOptions = materialOptions.Select(m => m.label).ToArray();
            }
        }
    }
}