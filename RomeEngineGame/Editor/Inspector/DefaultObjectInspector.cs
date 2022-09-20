using RomeEngine.IO;

namespace RomeEngineGame
{
    public sealed class DefaultObjectInspector : ObjectInspector
    {
        public override bool CanInspect(ISerializable inspectedObject)
        {
            return true;
        }
    }
}