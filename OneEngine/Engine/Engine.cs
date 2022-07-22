namespace OneEngine
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

        public void UpdateGraphics(IGraphics graphics)
        {
            Renderer.UpdateGraphics(graphics, Camera.Cameras[0]);
            Debug.DrawDebug(graphics, Camera.Cameras[0]);
        }
    }
}
