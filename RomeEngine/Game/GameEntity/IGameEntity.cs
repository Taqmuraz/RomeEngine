using RomeEngine.IO;

namespace RomeEngine
{
    public interface IGameEntity : IEventsHandler, ISerializable
    {
        string Name { get; }
        void Activate(IGameEntityActivityProvider activityProvider);
        void Deactivate(IGameEntityActivityProvider activityProvider);
    }
}
