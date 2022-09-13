using RomeEngine;

namespace OneEngineGame
{
    public interface IControlAgent
    {
        Vector2 InputMovement { get; }
        IControlAgentAction GetAction();
    }
}
