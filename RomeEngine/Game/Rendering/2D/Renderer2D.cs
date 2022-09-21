using System.Collections.Generic;
using System.Linq;

namespace RomeEngine
{
    public abstract class Renderer2D : Component
    {
        static List<Renderer2D> renderers = new List<Renderer2D>();
        protected static Renderer2DPass OutlinePass { get; } = new OutlineRendererPass() { Queue = 0 };
        protected static Renderer2DPass StandardPass { get; } = new StandardRendererPass() { Queue = 1 };

        protected virtual IEnumerable<Renderer2DPass> EnumeratePasses()
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

        protected virtual Matrix3x3 GetGraphicsTransform(Camera2D camera)
        {
            return camera.WorldToScreenMatrix * Transform.LocalToWorld;
        }

        public static void UpdateGraphics(IGraphics2D graphics, Camera2D camera)
        {
            graphics.Clear(Color32.black);
            graphics.Transform = camera.WorldToScreenMatrix;

            graphics.Brush = new SingleColorBrush(camera.ClearColor);
            graphics.DrawRect(Rect.FromCenterAndSize(camera.Transform.Position, camera.OrthographicSize));

            var renderers = Renderer2D.renderers.Where(r => r.IsInsideScreen(graphics, camera));
            var passes = renderers.SelectMany(r => r.EnumeratePasses()).Distinct().OrderBy(pass => pass.Queue);

            foreach (var pass in passes)
            {
                var renderersForPass = renderers.Where(r => r.EnumeratePasses().Contains(pass)).OrderByDescending(r => r.Queue);
                pass.Pass(graphics, camera, renderersForPass, r => r.GetGraphicsTransform(camera), (r, g, c) => r.GraphicsUpdate(g, c));
            }
        }
        void GraphicsUpdate(IGraphics2D graphics, Camera2D camera)
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

        protected abstract bool IsInsideScreen(IGraphics2D graphics, Camera2D camera);

        protected abstract void OnGraphicsUpdate(IGraphics2D graphics, Camera2D camera);
    }
}