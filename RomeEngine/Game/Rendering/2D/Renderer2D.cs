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

        protected virtual Matrix3x3 GetGraphicsTransform()
        {
            return Matrix3x3.identity;
        }

        public static void Update2DGraphics(IGraphics2D graphics)
        {
            graphics.Clear(Color32.black);
            graphics.Transform = Matrix3x3.identity;

            var passes = renderers.SelectMany(r => r.EnumeratePasses()).Distinct().OrderBy(pass => pass.Queue);

            foreach (var pass in passes)
            {
                var renderersForPass = renderers.Where(r => r.EnumeratePasses().Contains(pass)).OrderByDescending(r => r.Queue);
                pass.Pass(graphics, renderersForPass, r => r.GetGraphicsTransform(), (r, g) => r.GraphicsUpdate(g));
            }
        }
        void GraphicsUpdate(IGraphics2D graphics)
        {
            try
            {
                OnGraphicsUpdate(graphics);
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        protected abstract void OnGraphicsUpdate(IGraphics2D graphics);
    }
}