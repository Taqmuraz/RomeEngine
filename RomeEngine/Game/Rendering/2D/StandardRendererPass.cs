using System.Linq;
using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class StandardRendererPass : Renderer2DPass
    {
        public override void Pass(IGraphics2D graphics, IEnumerable<Renderer2D> renderers, Func<Renderer2D, Matrix3x3> graphicsTransform, Action<Renderer2D, IGraphics2D> drawCall)
        {
            graphics.Style = graphics.FillStyle;

            foreach (var renderer in renderers)
            {
                graphics.Transform = graphicsTransform(renderer);
                drawCall(renderer, graphics);
            }
        }
    }
}