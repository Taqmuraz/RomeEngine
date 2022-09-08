namespace RomeEngine.UI
{
    public sealed class CanvasText : ICanvasElement
    {
        string text;
        Rect rect;
        Color32 color;
        TextOptions options;

        public CanvasText(string text, Rect rect, Color32 color, TextOptions options)
        {
            this.text = text;
            this.rect = rect;
            this.color = color;
            this.options = options;
        }

        public void Draw(IGraphics graphics, Camera camera)
        {
            graphics.Brush = new SingleColorBrush(color);
            graphics.DrawText(text, rect, options);
        }
    }
}
