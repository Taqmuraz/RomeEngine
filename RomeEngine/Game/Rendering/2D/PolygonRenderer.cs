namespace RomeEngine
{
    public sealed class PolygonRenderer : Renderer2D
    {
        [SerializeField] public Color32 Color { get; set; } = Color32.black;
        [SerializeField] Vector2[] vertices;

        public void SetVertices(Vector2[] vertices)
        {
            if (vertices == null) throw new System.ArgumentNullException(nameof(vertices));
            this.vertices = vertices;
        }

        protected override void OnGraphicsUpdate(IGraphics2D graphics, Camera2D camera)
        {
            if (vertices != null && vertices.Length != 0)
            {
                graphics.Brush = new SingleColorBrush(Color);
                graphics.DrawPolygon(vertices);
            }
        }

        protected override bool IsInsideScreen(IGraphics2D graphics, Camera2D camera)
        {
            if (vertices != null && vertices.Length != 0)
            {
                var cameraVolume = camera.Volume;
                for (int i = 0; i < vertices.Length; i++)
                {
                    if (cameraVolume.Contains(vertices[i])) return true;
                }
            }
            return false;
        }
    }
}