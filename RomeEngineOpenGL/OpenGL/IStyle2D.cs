using RomeEngine;

namespace RomeEngineOpenGL
{
    interface IStyle2D : IGraphicsStyle
    {
        Matrix3x3 Transform { get; set; }
        IGraphicsBrush Brush { get; set; }

        void DrawEllipse(Vector2 center, Vector2 size);
        void DrawLine(Vector2 a, Vector2 b, float widthA, float widthB, bool smoothEnding);
        void DrawPolygon(Vector2[] points);
        void DrawRect(Rect rect);
        void DrawText(string text, Rect rect, Color32 color, TextOptions options);
    }
}