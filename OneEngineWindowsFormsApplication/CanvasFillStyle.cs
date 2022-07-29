using System.Drawing;

using OneEngine;

namespace OneEngineWindowsFormsApplication
{
    class CanvasFillStyle : CanvasStyleBase, ICanvasStyle
    {
        public void DrawPoint(Vector2 position, float radius)
        {
            DrawEllipse(new Vector2(position.x - radius, position.y - radius), new Vector2(radius * 2f, radius * 2f));
        }

        public void DrawLine(Vector2 a, Vector2 b)
        {
            Graphics.DrawLine(Pen, a, b);
        }
        public void DrawLine(Vector2 a, Vector2 b, float widthA, float widthB, bool smoothEnding)
        {
            var nonAllocLinePoints = AllocPoints(4);
            Vector3 diff = (b - a);
            float length = diff.length;
            Vector3 dir = diff.normalized;
            Matrix3x3 lineMatrix = Matrix3x3.New(dir, new Vector3(-dir.y, dir.x, 0f), a);
            float halfWidthA = widthA * 0.5f;
            float halfWidthB = widthB * 0.5f;
            nonAllocLinePoints[0] = lineMatrix.MultiplyPoint(new Vector2(0f, halfWidthA));
            nonAllocLinePoints[1] = lineMatrix.MultiplyPoint(new Vector2(length, halfWidthB));
            nonAllocLinePoints[2] = lineMatrix.MultiplyPoint(new Vector2(length, -halfWidthB));
            nonAllocLinePoints[3] = lineMatrix.MultiplyPoint(new Vector2(0f, -halfWidthA));
            Graphics.FillPolygon(Brush, nonAllocLinePoints);
            if (smoothEnding)
            {
                DrawEllipse(a, new Vector2(widthA, widthA));
                DrawEllipse(b, new Vector2(widthB, widthB));
            }
        }

        public void DrawEllipse(Vector2 center, Vector2 size)
        {
            Graphics.FillEllipse(Brush, new RectangleF(center - size * 0.5f, size));
        }

        public void DrawRect(Rect rect)
        {
            Graphics.FillRectangle(Brush, rect);
        }

        public void DrawPolygon(Vector2[] points)
        {
            var pointsF = AllocPoints(points.Length);
            for (int i = 0; i < points.Length; i++) pointsF[i] = points[i];
            Graphics.FillPolygon(Brush, pointsF);
        }

        public void DrawText(string text, Rect rect)
        {
            DrawInReversedScale(() => Graphics.DrawString(text, SystemFonts.DefaultFont, Brush, rect));
        }
    }
}
