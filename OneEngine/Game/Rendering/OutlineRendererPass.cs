using System;

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

        float outlineSize = 0.1f;

        public override void Pass(IGraphics graphics, Camera camera, ReadOnlyArrayList<Renderer> renderers, Action<Renderer, IGraphics, Camera> drawCall)
        {
            graphics = new BlackBrushGraphics(graphics);
            graphics.Style = graphics.OutlineStyle;

            if (Input.GetKey(KeyCode.Z)) outlineSize -= 1f * Time.deltaTime;
            if (Input.GetKey(KeyCode.C)) outlineSize += 1f * Time.deltaTime;
            outlineSize = outlineSize.Clamp(0f, 1f);

            foreach (var renderer in renderers)
            {
                graphics.Transform = camera.WorldToScreenMatrix * renderer.Transform.LocalToWorld;
                drawCall(renderer, graphics, camera);
            }
        }
    }
}