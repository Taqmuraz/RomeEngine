namespace RomeEngine
{
    public interface IGameObjectActivityProvider
    {
        void Activate(IGameObject gameObject);
        void Deactivate(IGameObject gameObject);
    }
}
