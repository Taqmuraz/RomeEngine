namespace OneEngine
{
    public sealed class LineRenderer : Renderer
    {
        Line[] lines;
        public void SetLines(params Line[] lines)
        {
            this.lines = lines;
        }
        protected override void OnGraphicsUpdate(IGraphics graphics, Camera camera)
        {
            foreach (var line in lines)
            {
                graphics.Brush = new SingleColorBrush(line.Color);
                graphics.DrawLine(line.PointA, line.PointB, line.WidthA, line.WidthB);
            }
        }
    }
}