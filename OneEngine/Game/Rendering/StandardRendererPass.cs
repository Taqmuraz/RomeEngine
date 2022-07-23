using System.Linq;
using System;
using System.Collections.Generic;

namespace OneEngine
{
    public sealed class StandardRendererPass : RendererPass
    {
        public override void Pass(IGraphics graphics, Camera camera, IEnumerable<Renderer> renderers, Action<Renderer, IGraphics, Camera> drawCall)
        {
            graphics.Style = graphics.FillStyle;

            foreach (var renderer in renderers.OrderBy(r => r.Queue))
            {
                graphics.Transform = camera.WorldToScreenMatrix * renderer.Transform.LocalToWorld;
                drawCall(renderer, graphics, camera);
            }
        }
    }
}