using RomeEngine;

namespace RomeEngineEditor
{
    public interface IControlAgent
    {
        Vector3 InputMovement { get; }
        IControlAction GetAction();
    }
}
