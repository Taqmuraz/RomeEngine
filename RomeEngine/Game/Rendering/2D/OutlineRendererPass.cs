using System;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class OutlineRendererPass : Renderer2DPass
    {
        public class BlackBrushGraphics : GraphicsAdapter
        {
            public BlackBrushGraphics(IGraphics2D provider, int width = 6) : base(provider)
            {
                provider.Brush = new SingleColorBrush(Color32.black, width);
            }

            public override IGraphicsBrush Brush { get => brush; set => brush = value; }
            public override IGraphicsStyle Style { get => base.Style; set => base.Style = value; }
            IGraphicsBrush brush;
        }

        public override void Pass(IGraphics2D graphics, Camera2D camera, IEnumerable<Renderer2D> renderers, Func<Renderer2D, Matrix3x3> graphicsTransform, Action<Renderer2D, IGraphics2D, Camera2D> drawCall)
        {
            graphics = new BlackBrushGraphics(graphics);
            graphics.Style = graphics.OutlineStyle;

            foreach (var renderer in renderers)
            {
                graphics.Transform = graphicsTransform(renderer);
                drawCall(renderer, graphics, camera);
            }
        }
    }
}