namespace RomeEngine
{
    public sealed class Engine : IEngine
    {
        public static Engine Instance { get; private set; }
        public IEngineRuntine Runtime { get; private set; }

        public void Initialize(IEngineRuntine runtine)
        {
            Instance = this;
            Runtime = runtine;

            Time.StartTime();
            runtine.SetInputHandler(new Input());

            GameScenes.LoadScene(1);
        }

        public void UpdateGameState()
        {
            try
            {
                Game.UpdateGameState();
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        public void UpdateGraphics(IGraphics2D graphics2D, IGraphics graphics, IGraphicsContext context)
        {
            Game.UpdateGraphics(graphics2D, graphics, context);
            Debug.DrawDebug(graphics2D);
        }
    }
}
