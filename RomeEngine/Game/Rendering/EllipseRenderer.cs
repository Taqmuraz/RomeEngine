using RomeEngine.UI;

namespace RomeEngine
{
    public sealed class EllipseRenderer : Renderer, IHandlable
    {
        [SerializeField] public Color32 Color { get; set; } = Color32.black;
        [SerializeField] public Vector2 Offset { get; set; }
        [SerializeField] public Vector2 Scale { get; set; } = Vector2.one;
        [SerializeField] public float Rotation { get; set; }

        protected override Matrix3x3 GetGraphicsTransform(Camera camera)
        {
            return camera.WorldToScreenMatrix * Transform.LocalToWorld * Matrix3x3.TransformMatrix(Offset, Scale, Rotation);
        }

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

        public (Vector2, Vector2)[] GetHandleLines()
        {
            var l2w = Transform.LocalToWorld;
            return new[] { ((Vector2)l2w.Column_2, l2w.MultiplyPoint(Offset)) };
        }
    }
}