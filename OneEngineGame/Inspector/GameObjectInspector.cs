using OneEngine;
using OneEngine.IO;

namespace OneEngineGame
{
    public sealed class GameObjectInspector : ObjectInspector
    {
        public override bool CanInspect(ISerializable inspectedObject)
        {
            return inspectedObject is GameObject;
        }
        protected override void AfterInspect(ISerializable inspectedObject, InspectorMenu inspectorMenu, EditorCanvas canvas)
        {
            var gameObject = (GameObject)inspectedObject;
            var components = gameObject.GetComponents();

            foreach (var component in components)
            {
                Rect labelRect;
                if (component is Transform)
                {
                    labelRect = inspectorMenu.GetNextRect();
                }
                else
                {
                    inspectorMenu.AllocateField(out labelRect, out Rect valueRect);
                    if (canvas.DrawButton("Destroy", valueRect, new TextOptions() { FontSize = 20f, Alignment = TextAlignment.MiddleCenter }))
                    {
                        component.Destroy();
                    }
                }
                canvas.DrawRectWithText(component.GetType().Name, labelRect, new TextOptions() { FontSize = 20f, Alignment = TextAlignment.MiddleCenter });

                inspectorMenu.GetObjectInspector(component).Inspect(component, inspectorMenu, canvas);
            }

            inspectorMenu.GetNextRect();
            var buttonRect = inspectorMenu.GetNextRect();
            if (canvas.DrawButton("Add component", buttonRect, new TextOptions() { FontSize = 25f, Alignment = TextAlignment.MiddleCenter }))
            {
                EditorMenu.ShowMenu<StringInputMenu>(canvas, menu =>
                {
                    var type = TypesMap.GetType(menu.InputString);
                    if (type != null && typeof(Component).IsAssignableFrom(type)) ((GameObject)inspectedObject).AddComponent(type);
                }).WithHeader("Component type name");
            }
        }
    }
}