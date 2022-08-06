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
            inspectorMenu.GetNextRect();
            var buttonRect = inspectorMenu.GetNextRect();
            if (canvas.DrawButton("Add component", buttonRect, new TextOptions() { FontSize = 25f, Alignment = TextAlignment.MiddleCenter }))
            {
                var inputMenu = EditorMenu.ShowMenu<StringInputMenu>(canvas, menu =>
                {
                    var type = TypesMap.GetType(menu.InputString);
                    if (type != null && typeof(Component).IsAssignableFrom(type)) ((GameObject)inspectedObject).AddComponent(type);
                });
                inputMenu.Header = "Component type name";
            }
        }
    }
}