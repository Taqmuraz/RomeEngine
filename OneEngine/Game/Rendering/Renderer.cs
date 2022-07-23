using System.Collections.Generic;

namespace OneEngine
{
    public abstract class Renderer : Component
    {
        static List<Renderer> renderers = new List<Renderer>();

        static List<RendererPass> passes = new List<RendererPass>() { new OutlineRendererPass(), new StandardRendererPass() };

        public int Queue { get; set; }

        [BehaviourEvent]
        void Start()
        {
            renderers.Add(this);
        }
        [BehaviourEvent]
        void OnDestroy()
        {
            renderers.Remove(this);
        }

        public static void UpdateGraphics(IGraphics graphics, Camera camera)
        {
            graphics.Clear(camera.ClearColor);
            camera.Transform.LocalPosition += Input.GetWASD() * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Q)) camera.OrthographicSize *= 2f;
            if (Input.GetKeyDown(KeyCode.E)) camera.OrthographicSize *= 0.5f;

            foreach (var pass in passes)
            {
                pass.Pass(graphics, camera, renderers, (r, g, c) => r.OnGraphicsUpdate(g, c));
            }
        }

        protected abstract void OnGraphicsUpdate(IGraphics graphics, Camera camera);
    }
}