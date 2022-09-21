namespace RomeEngine.UI
{
    public sealed class CanvasRect : ICanvasElement
    {
        Rect rect;
        Color32 color;

        public CanvasRect(Rect rect, Color32 color)
        {
            this.rect = rect;
            this.color = color;
        }

        public void Draw(IGraphics2D graphics)
        {
            graphics.Brush = new SingleColorBrush(color);
            graphics.DrawRect(rect);
        }
    }
}
