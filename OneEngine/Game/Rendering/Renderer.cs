using System.Collections.Generic;
using System.Linq;

namespace OneEngine
{
    public abstract class Renderer : Component
    {
        static List<Renderer> renderers = new List<Renderer>();

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
            graphics.Style = graphics.FillStyle;
            graphics.Clear(camera.ClearColor);
            camera.Transform.LocalPosition += Input.GetWASD() * Time.deltaTime * 10f;

            if (Input.GetKeyDown(KeyCode.Q)) camera.OrthographicSize *= 2f;
            if (Input.GetKeyDown(KeyCode.E)) camera.OrthographicSize *= 0.5f;

            foreach (var renderer in renderers.OrderBy(r => r.Queue))
            {
                graphics.Transform = camera.WorldToScreenMatrix * renderer.Transform.LocalToWorld;
                renderer.OnGraphicsUpdate(graphics, camera);
            }
        }

        protected abstract void OnGraphicsUpdate(IGraphics graphics, Camera camera);
    }
}