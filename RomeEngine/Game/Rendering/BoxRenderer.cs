namespace RomeEngine
{
    public sealed class BoxRenderer : Renderer
    {
        [SerializeField] public Color32 Color { get; set; } = Color32.black;
        [SerializeField] public Vector2 Offset { get; set; }
        [SerializeField] public Vector2 Scale { get; set; } = Vector2.one;
        [SerializeField] public float Rotation { get; set; }

        protected override Matrix3x3 GetGraphicsTransform(Camera camera)
        {
            return camera.WorldToScreenMatrix * Transform.LocalToWorld * Matrix3x3.TransformMatrix(Offset, Scale, Rotation);
        }

        protected override bool IsInsideScreen(IGraphics graphics, Camera camera)
        {
            var scale = Transform.Scale;
            return camera.Volume.IntersectsWith(Rect.FromCenterAndSize(Transform.Position, Vector2.one * Mathf.Max(scale.x, scale.y)));
        }

        protected override void OnGraphicsUpdate(IGraphics graphics, Camera camera)
        {
            graphics.Brush = new SingleColorBrush(Color);
            graphics.DrawLine(new Vector2(-0.5f, 0f), new Vector2(0.5f, 0f), 1f, 1f, false);
        }
    }
}