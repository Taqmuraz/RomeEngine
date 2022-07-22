namespace OneEngine
{
    public sealed class TextRenderer : Renderer
    {
        public string Text { get; set; } = "NewText";
        public int FontSize { get; set; } = 10;

        protected override void OnGraphicsUpdate(IGraphics graphics, Camera camera)
        {
            graphics.DrawText(Vector2.zero, Text, FontSize);
        }
    }
}