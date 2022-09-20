using RomeEngine;

namespace RomeEngineGame
{
    public interface IControlAgent
    {
        Vector2 InputMovement { get; }
        IControlAction GetAction();
    }
}
