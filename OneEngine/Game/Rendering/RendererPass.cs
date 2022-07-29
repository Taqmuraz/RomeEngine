using System;
using System.Collections.Generic;

namespace OneEngine
{
    public abstract class RendererPass
    {
        public abstract void Pass(IGraphics graphics, Camera camera, IEnumerable<Renderer> renderers, Func<Renderer, Matrix3x3> graphicsTransform, Action<Renderer, IGraphics, Camera> drawCall);
        public int Queue { get; set; }
    }
}