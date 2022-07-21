namespace OneEngine
{
    public interface IGraphics
    {
        Matrix3x3 Transform { get; set; }

        IGraphicsBrush Brush { get; set; }
        IGraphicsStyle Style { get; set; }

        IGraphicsStyle DrawStyle { get; }
        IGraphicsStyle FillStyle { get; }

        void Clear(Color32 color);
        void DrawPoint(Vector2 position, float radius);
        void DrawLine(Vector2 a, Vector2 b);
        void DrawLine(Vector2 a, Vector2 b, float widthA, float widthB);
        void DrawText(Vector2 position, string text, int fontSize);
        void DrawEllipse(Vector2 center, Vector2 size);
        void DrawRect(Rect rect);
        void DrawPolygon(Vector2[] points);

        Vector2 MeasureText(string text, int fontSize);
    }
}
