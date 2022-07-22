namespace OneEngine
{
    public interface IEngine
    {
        void Initialize(IEngineRuntine runtine);
        void UpdateGameState();
        void UpdateGraphics(IGraphics graphics);
    }
}
