using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public abstract class Renderer2DPass
    {
        public abstract void Pass(IGraphics2D graphics, IEnumerable<Renderer2D> renderers, Func<Renderer2D, Matrix3x3> graphicsTransform, Action<Renderer2D, IGraphics2D> drawCall);
        public int Queue { get; set; }
    }
}