using System;
using System.Collections.Generic;

namespace OneEngine
{
    public abstract class RendererPass
    {
        public abstract void Pass(IGraphics graphics, Camera camera, IEnumerable<Renderer> renderers, Action<Renderer, IGraphics, Camera> drawCall);
    }
}