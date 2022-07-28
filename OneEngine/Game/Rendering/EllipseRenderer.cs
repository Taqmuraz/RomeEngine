namespace OneEngine
{
    public sealed class EllipseRenderer : Renderer
    {
        [SerializeField] public Color32 Color { get; set; } = Color32.black;

        protected override void OnGraphicsUpdate(IGraphics graphics, Camera camera)
        {
            graphics.Brush = new SingleColorBrush(Color);
            graphics.DrawEllipse(Vector2.zero, Vector2.one);
        }

        protected override bool IsInsideScreen(IGraphics graphics, Camera camera)
        {
            var scale = Transform.Scale;
            return camera.Volume.IntersectsWith(Rect.FromCenterAndSize(Transform.Position, Vector2.one * Mathf.Max(scale.x, scale.y)));
        }
    }
}