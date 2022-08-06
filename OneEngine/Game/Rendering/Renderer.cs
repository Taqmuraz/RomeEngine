using System.Collections.Generic;
using System.Linq;

namespace OneEngine
{
    public abstract class Renderer : Component
    {
        static List<Renderer> renderers = new List<Renderer>();
        protected static RendererPass OutlinePass { get; } = new OutlineRendererPass() { Queue = 0 };
        protected static RendererPass StandardPass { get; } = new StandardRendererPass() { Queue = 1 };

        protected virtual IEnumerable<RendererPass> EnumeratePasses()
        {
            yield return OutlinePass;
            yield return StandardPass;
        }

        [SerializeField] public int Queue { get; set; }

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

        protected virtual Matrix3x3 GetGraphicsTransform(Camera camera)
        {
            return camera.WorldToScreenMatrix * Transform.LocalToWorld;
        }

        public static void UpdateGraphics(IGraphics graphics, Camera camera)
        {
            graphics.Clear(Color32.black);
            graphics.Transform = camera.WorldToScreenMatrix;

            graphics.Brush = new SingleColorBrush(camera.ClearColor);
            graphics.DrawRect(Rect.FromCenterAndSize(camera.Transform.Position, camera.OrthographicSize));

            var renderers = Renderer.renderers.Where(r => r.IsInsideScreen(graphics, camera));
            var passes = renderers.SelectMany(r => r.EnumeratePasses()).Distinct().OrderBy(pass => pass.Queue);

            foreach (var pass in passes)
            {
                var renderersForPass = renderers.Where(r => r.EnumeratePasses().Contains(pass)).OrderByDescending(r => r.Queue);
                pass.Pass(graphics, camera, renderersForPass, r => r.GetGraphicsTransform(camera), (r, g, c) => r.GraphicsUpdate(g, c));
            }
        }
        void GraphicsUpdate(IGraphics graphics, Camera camera)
        {
            try
            {
                OnGraphicsUpdate(graphics, camera);
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        protected abstract bool IsInsideScreen(IGraphics graphics, Camera camera);

        protected abstract void OnGraphicsUpdate(IGraphics graphics, Camera camera);
    }
}