using System;

namespace OneEngine
{
    public abstract class RendererPass
    {
        public abstract void Pass(IGraphics graphics, Camera camera, ReadOnlyArrayList<Renderer> renderers, Action<Renderer, IGraphics, Camera> drawCall);
    }
}