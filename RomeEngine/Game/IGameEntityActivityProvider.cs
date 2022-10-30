namespace RomeEngine
{
    public interface IGameEntityActivityProvider
    {
        void Activate(IGameEntity entity);
        void Deactivate(IGameEntity entity);
    }
}