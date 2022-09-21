namespace RomeEngine
{
    public sealed class TextRenderer : Renderer2D
    {
        public string Text { get; set; } = "NewText";
        public Rect Rect { get; set; }
        public TextOptions TextOptions { get; set; } = TextOptions.Default;

        protected override void OnGraphicsUpdate(IGraphics2D graphics, Camera2D camera)
        {
            graphics.DrawText(Text, Rect, TextOptions);
        }
        protected override bool IsInsideScreen(IGraphics2D graphics, Camera2D camera)
        {
            return new Rect(Transform.Position, Transform.Scale).IntersectsWith(camera.Volume);
        }
    }
}