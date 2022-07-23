using System.Collections.Generic;
using System.Linq;

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
            graphics.Clear(Color32.black);
            graphics.Transform = camera.WorldToScreenMatrix;

            graphics.Brush = new SingleColorBrush(camera.ClearColor);
            graphics.DrawRect(Rect.FromCenterAndSize(camera.Transform.Position, camera.OrthographicSize));

            camera.Transform.LocalPosition += Input.GetWASD() * Time.deltaTime * 5f;

            if (Input.GetKeyDown(KeyCode.Q)) camera.OrthographicMultiplier *= 2f;
            if (Input.GetKeyDown(KeyCode.E)) camera.OrthographicMultiplier *= 0.5f;

            var renderers = Renderer.renderers.Where(r => r.IsInsideScreen(graphics, camera));

            foreach (var pass in passes)
            {
                pass.Pass(graphics, camera, renderers, (r, g, c) => r.OnGraphicsUpdate(g, c));
            }
        }

        protected abstract bool IsInsideScreen(IGraphics graphics, Camera camera);

        protected abstract void OnGraphicsUpdate(IGraphics graphics, Camera camera);
    }
}