using RomeEngine;

namespace RomeEngineGame
{
    public interface IControlAgent
    {
        Vector3 InputMovement { get; }
        IControlAction GetAction();
    }
}
