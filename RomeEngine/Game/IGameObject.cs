using RomeEngine.IO;

namespace RomeEngine
{
    public interface IGameObject : IGameEntity, IGameEntityActivityProvider
    {
        string Name { get; }
        ITransform Transform { get; }
        void Activate(IGameObjectActivityProvider activityProvider);
        void Deactivate(IGameObjectActivityProvider activityProvider);
        TComponent GetComponent<TComponent>();
        TComponent AddComponent<TComponent>() where TComponent : Component, IInitializable<IGameObject>, new();
        Component[] GetComponents();
    }
}