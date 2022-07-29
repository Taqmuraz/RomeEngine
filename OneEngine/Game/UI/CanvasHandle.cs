namespace OneEngine.UI
{
    public sealed class CanvasHandle : ICanvasElement
    {
        Vector2 position;
        float radius;
        Color32 color;

        public CanvasHandle(Vector2 position, float radius, Color32 color)
        {
            this.position = position;
            this.radius = radius;
            this.color = color;
        }

        public void Draw(IGraphics graphics, Camera camera)
        {
            graphics.Style = graphics.OutlineStyle;
            graphics.Brush = new SingleColorBrush(color);
            graphics.DrawEllipse(position, new Vector2(radius, radius) * 2f);
        }
    }
}
