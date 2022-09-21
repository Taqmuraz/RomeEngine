namespace RomeEngine.UI
{
    public sealed class CanvasCircle : ICanvasElement
    {
        Vector2 position;
        float radius;
        Color32 color;

        public CanvasCircle(Vector2 position, float radius, Color32 color)
        {
            this.position = position;
            this.radius = radius;
            this.color = color;
        }

        public void Draw(IGraphics2D graphics, Camera2D camera)
        {
            graphics.Style = graphics.OutlineStyle;
            graphics.Brush = new SingleColorBrush(color);
            graphics.DrawEllipse(position, new Vector2(radius, radius) * 2f);
        }
    }
}
