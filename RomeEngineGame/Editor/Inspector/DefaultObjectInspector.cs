using RomeEngine.IO;

namespace OneEngineGame
{
    public sealed class DefaultObjectInspector : ObjectInspector
    {
        public override bool CanInspect(ISerializable inspectedObject)
        {
            return true;
        }
    }
}