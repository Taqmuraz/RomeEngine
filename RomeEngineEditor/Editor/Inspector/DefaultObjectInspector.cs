using RomeEngine.IO;

namespace RomeEngineEditor
{
    public sealed class DefaultObjectInspector : ObjectInspector
    {
        public override bool CanInspect(ISerializable inspectedObject)
        {
            return true;
        }
    }
}