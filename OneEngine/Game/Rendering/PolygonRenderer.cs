namespace OneEngine
{
    public sealed class PolygonRenderer : Renderer
    {
        public Color32 Color { get; set; } = Color32.black;
        Vector2[] vertices;
        public void SetVertices(Vector2[] vertices)
        {
            this.vertices = vertices;
        }

        protected override void OnGraphicsUpdate(IGraphics graphics, Camera camera)
        {
            graphics.DrawPolygon(vertices);
        }
    }
}