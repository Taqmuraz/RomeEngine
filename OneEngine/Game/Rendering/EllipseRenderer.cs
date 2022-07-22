namespace OneEngine
{
    public sealed class EllipseRenderer : Renderer
    {
        public Color32 Color { get; set; } = Color32.black;

        protected override void OnGraphicsUpdate(IGraphics graphics, Camera camera)
        {
            graphics.Brush = new SingleColorBrush(Color);
            graphics.DrawEllipse(Vector2.zero, Vector2.one);
        }
    }
}