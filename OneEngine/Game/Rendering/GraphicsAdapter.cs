namespace OneEngine
{
    public class GraphicsAdapter : IGraphics
    {
        protected IGraphics Provider { get; }

        public GraphicsAdapter(IGraphics provider)
        {
            this.Provider = provider;
        }

        public virtual Matrix3x3 Transform { get => Provider.Transform; set => Provider.Transform = value; }
        public virtual IGraphicsBrush Brush { get => Provider.Brush; set => Provider.Brush = value; }
        public virtual IGraphicsStyle Style { get => Provider.Style; set => Provider.Style = value; }

        public virtual IGraphicsStyle OutlineStyle => Provider.OutlineStyle;

        public virtual IGraphicsStyle FillStyle => Provider.FillStyle;

        public virtual void Clear(Color32 color)
        {
            Provider.Clear(color);
        }

        public virtual void DrawPoint(Vector2 position, float radius)
        {
            Provider.DrawPoint(position, radius);
        }

        public virtual void DrawLine(Vector2 a, Vector2 b)
        {
            Provider.DrawLine(a, b);
        }

        public virtual void DrawLine(Vector2 a, Vector2 b, float widthA, float widthB, bool smoothEnding)
        {
            Provider.DrawLine(a, b, widthA, widthB, smoothEnding);
        }

        public virtual void DrawText(Vector2 position, string text, float fontSize)
        {
            Provider.DrawText(position, text, fontSize);
        }

        public virtual void DrawEllipse(Vector2 center, Vector2 size)
        {
            Provider.DrawEllipse(center, size);
        }

        public virtual void DrawRect(Rect rect)
        {
            Provider.DrawRect(rect);
        }

        public virtual void DrawPolygon(Vector2[] points)
        {
            Provider.DrawPolygon(points);
        }

        public virtual Vector2 MeasureText(string text, int fontSize)
        {
            return Provider.MeasureText(text, fontSize);
        }
    }
}