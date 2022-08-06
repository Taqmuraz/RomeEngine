using OneEngine.IO;

namespace OneEngineGame
{
    public interface IObjectInspector
    {
        bool CanInspect(ISerializable inspectedObject);
        void Inspect(ISerializable inspectedObject, InspectorMenu inspectorMenu, EditorCanvas canvas);
    }
}