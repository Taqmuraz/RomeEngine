using RomeEngine.IO;

namespace RomeEngineGame
{
    public abstract class ObjectInspector : IObjectInspector
    {
        public void Inspect(ISerializable inspectedObject, InspectorMenu inspectorMenu, EditorCanvas canvas)
        {
            BeforeInspect(inspectedObject, inspectorMenu, canvas);
            foreach (var field in inspectedObject.EnumerateFields())
            {
                if (!field.HideInInspector) inspectorMenu.GetFieldInspector(field.Type).Inspect(field.Name, field.Value, field.Setter, field.Type, canvas, inspectorMenu);
            }
            AfterInspect(inspectedObject, inspectorMenu, canvas);
        }
        protected virtual void BeforeInspect(ISerializable inspectedObject, InspectorMenu inspectorMenu, EditorCanvas canvas) { }
        protected virtual void AfterInspect(ISerializable inspectedObject, InspectorMenu inspectorMenu, EditorCanvas canvas) { }

        public abstract bool CanInspect(ISerializable inspectedObject);
    }
}