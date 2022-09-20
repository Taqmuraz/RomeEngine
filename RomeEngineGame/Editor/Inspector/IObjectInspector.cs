using RomeEngine.IO;

namespace RomeEngineGame
{
    public interface IObjectInspector
    {
        bool CanInspect(ISerializable inspectedObject);
        void Inspect(ISerializable inspectedObject, InspectorMenu inspectorMenu, EditorCanvas canvas);
    }
}