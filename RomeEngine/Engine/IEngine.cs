namespace RomeEngine
{
    public interface IEngine
    {
        void Initialize(IEngineRuntine runtine);
        void UpdateGameState();
        void UpdateGraphics(IGraphics2D graphics);
    }
}
