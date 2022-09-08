using System.Linq;
using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class StandardRendererPass : RendererPass
    {
        public override void Pass(IGraphics graphics, Camera camera, IEnumerable<Renderer> renderers, Func<Renderer, Matrix3x3> graphicsTransform, Action<Renderer, IGraphics, Camera> drawCall)
        {
            graphics.Style = graphics.FillStyle;

            foreach (var renderer in renderers)
            {
                graphics.Transform = graphicsTransform(renderer);
                drawCall(renderer, graphics, camera);
            }
        }
    }
}