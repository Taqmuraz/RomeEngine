namespace RomeEngine.UI
{
    public sealed class CanvasLine : ICanvasElement
    {
        Vector2 a;
        Vector2 b;
        Color32 color;
        float width;

        public CanvasLine(Vector2 a, Vector2 b, Color32 color, int width)
        {
            this.a = a;
            this.b = b;
            this.color = color;
            this.width = width;
        }

        public void Draw(IGraphics2D graphics)
        {
            graphics.Brush = new SingleColorBrush(color, (int)width);
            graphics.DrawLine(a, b);
        }
    }
}
