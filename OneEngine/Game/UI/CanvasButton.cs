namespace OneEngine.UI
{
    public sealed class CanvasButton : ICanvasElement
    {
        Rect rect;
        string text;
        Color32 textColor;
        Color32 buttonColor;
        TextOptions options;

        public CanvasButton(string text, Rect rect, Color32 textColor, Color32 buttonColor, TextOptions options)
        {
            this.rect = rect;
            this.text = text;
            this.textColor = textColor;
            this.buttonColor = buttonColor;
            this.options = options;
        }

        public void Draw(IGraphics graphics, Camera camera)
        {
            graphics.Brush = new SingleColorBrush(buttonColor);
            graphics.DrawRect(rect);
            graphics.Brush = new SingleColorBrush(textColor);
            graphics.DrawText(text, rect, options);
        }
    }
}
