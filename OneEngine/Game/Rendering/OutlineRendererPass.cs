using System;
using System.Collections.Generic;

namespace OneEngine
{
    public sealed class OutlineRendererPass : RendererPass
    {
        public class BlackBrushGraphics : GraphicsAdapter
        {
            public BlackBrushGraphics(IGraphics provider) : base(provider)
            {
                provider.Brush = new SingleColorBrush(Color32.black, 6);
            }

            public override IGraphicsBrush Brush { get => brush; set => brush = value; }
            public override IGraphicsStyle Style { get => base.Style; set => base.Style = value; }
            IGraphicsBrush brush;
        }

        public override void Pass(IGraphics graphics, Camera camera, IEnumerable<Renderer> renderers, Func<Renderer, Matrix3x3> graphicsTransform, Action<Renderer, IGraphics, Camera> drawCall)
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