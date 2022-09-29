using System.Drawing;

using RomeEngine;

namespace RomeEngine.SystemDrawing
{
    public interface ICanvasStyle : IGraphicsStyle
    {
        void DrawPoint(Vector2 position, float radius);
        void DrawLine(Vector2 a, Vector2 b);
        void DrawLine(Vector2 a, Vector2 b, float widthA, float widthB, bool smoothEnding);
        void DrawText(string text, Rect rect, TextOptions options);
        void DrawEllipse(Vector2 center, Vector2 size);
        void DrawRect(Rect rect);
        void DrawPolygon(Vector2[] points);

        Graphics Graphics { get; set; }
        SolidBrush Brush { get; set; }
        Pen Pen { get; set; }
        Matrix3x3 Transform { get; set; }

        void Setup();
    }
}
