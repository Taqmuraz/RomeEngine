namespace RomeEngine
{
    public class GraphicsAdapter : IGraphics2D
    {
        protected IGraphics2D Provider { get; }

        public GraphicsAdapter(IGraphics2D provider)
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

        public virtual void DrawText(string text, Rect rect, TextOptions options)
        {
            Provider.DrawText(text, rect, options);
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

        public virtual Vector2 MeasureText(string text, float fontSize)
        {
            return Provider.MeasureText(text, fontSize);
        }
    }
}