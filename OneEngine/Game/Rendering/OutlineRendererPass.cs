using System;
using System.Collections.Generic;

namespace OneEngine
{
    public sealed class OutlineRendererPass : RendererPass
    {
        class BlackBrushGraphics : GraphicsAdapter
        {
            public BlackBrushGraphics(IGraphics provider) : base(provider)
            {
                provider.Brush = new SingleColorBrush(Color32.black, 6);
            }

            public override IGraphicsBrush Brush { get => brush; set => brush = value; }
            public override IGraphicsStyle Style { get => base.Style; set => base.Style = value; }
            IGraphicsBrush brush;
        }

        public override void Pass(IGraphics graphics, Camera camera, IEnumerable<Renderer> renderers, Action<Renderer, IGraphics, Camera> drawCall)
        {
            graphics = new BlackBrushGraphics(graphics);
            graphics.Style = graphics.OutlineStyle;

            foreach (var renderer in renderers)
            {
                graphics.Transform = camera.WorldToScreenMatrix * renderer.Transform.LocalToWorld;
                drawCall(renderer, graphics, camera);
            }
        }
    }
}