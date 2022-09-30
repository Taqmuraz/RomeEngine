using System;

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

        public void UpdateGraphics2D(IGraphics2D graphics2D)
        {
            Game.UpdateGraphics2D(graphics2D);
            Debug.DrawDebug(graphics2D);
        }
        public void UpdateGraphics3D(IGraphics graphics, IGraphicsContext context)
        {
            Game.UpdateGraphics3D(graphics, context);
        }

        public static void Quit()
        {
            Instance.Runtime.Close();
        }
    }
}
