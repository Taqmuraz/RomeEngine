namespace RomeEngine
{
    public interface IEngine
    {
        void Initialize(IEngineRuntine runtine);
        void UpdateGameState();
        void UpdateGraphics2D(IGraphics2D graphics2D);
        void UpdateGraphics3D(IGraphics graphics, IGraphicsContext context);
    }
}
