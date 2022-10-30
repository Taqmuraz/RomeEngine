using RomeEngine.IO;

namespace RomeEngine
{
    public interface IGameObject : IGameEntity, IGameEntityActivityProvider
    {
        string Name { get; }
        ITransform Transform { get; }
        TComponent GetComponent<TComponent>();
        TComponent AddComponent<TComponent>() where TComponent : Component, IInitializable<IGameObject>, new();
        Component[] GetComponents();
    }
}