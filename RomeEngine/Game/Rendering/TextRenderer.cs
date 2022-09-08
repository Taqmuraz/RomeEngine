namespace RomeEngine
{
    public sealed class TextRenderer : Renderer
    {
        public string Text { get; set; } = "NewText";
        public Rect Rect { get; set; }
        public TextOptions TextOptions { get; set; } = TextOptions.Default;

        protected override void OnGraphicsUpdate(IGraphics graphics, Camera camera)
        {
            graphics.DrawText(Text, Rect, TextOptions);
        }
        protected override bool IsInsideScreen(IGraphics graphics, Camera camera)
        {
            return new Rect(Transform.Position, Transform.Scale).IntersectsWith(camera.Volume);
        }
    }
}