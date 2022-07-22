using System.Collections.Generic;

namespace OneEngine
{
    public abstract class Renderer : Component
    {
        static List<Renderer> renderers = new List<Renderer>();

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
            foreach (var renderer in renderers)
            {
                graphics.Transform = camera.WorldToScreenMatrix * renderer.transform.localToWorld;
                renderer.OnGraphicsUpdate(graphics, camera);
            }
        }

        protected abstract void OnGraphicsUpdate(IGraphics graphics, Camera camera);
    }
}