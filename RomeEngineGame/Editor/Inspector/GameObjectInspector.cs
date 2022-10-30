using RomeEngine;
using RomeEngine.IO;
using System.Linq;

namespace RomeEngineGame
{
    public sealed class GameObjectInspector : ObjectInspector
    {
        public override bool CanInspect(ISerializable inspectedObject)
        {
            return inspectedObject is GameObject;
        }
        protected override void AfterInspect(ISerializable inspectedObject, InspectorMenu inspectorMenu, EditorCanvas canvas)
        {
            var gameObject = (IGameObject)inspectedObject;
            var components = gameObject.GetComponents();

            foreach (var component in components)
            {
                inspectorMenu.AllocateField(out Rect labelRect, out Rect valueRect);
                if (canvas.DrawButton("Destroy", valueRect, new TextOptions() { FontSize = 20f, Alignment = TextAlignment.MiddleCenter }))
                {
                    component.Destroy();
                }

                canvas.DrawRectWithText(component.GetType().Name, labelRect, new TextOptions() { FontSize = 20f, Alignment = TextAlignment.MiddleCenter });

                inspectorMenu.GetObjectInspector(component).Inspect(component, inspectorMenu, canvas);
            }

            inspectorMenu.GetNextRect();
            var buttonRect = inspectorMenu.GetNextRect();
            if (canvas.DrawButton("Add component", buttonRect, new TextOptions() { FontSize = 25f, Alignment = TextAlignment.MiddleCenter }))
            {
                var types = TypesMap.FindTypes(t => typeof(Component).IsAssignableFrom(t) && !t.IsAbstract);

                EditorMenu.ShowMenu<DropdownMenu>(canvas, menu =>
                {
                    var type = types[menu.SelectedOption];
                    ((GameObject)inspectedObject).AddComponent(type);
                }).DropdownOptions = types.Select(t => t.Name).ToArray();
            }
        }
    }
}