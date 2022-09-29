using System.Drawing;

using RomeEngine;

namespace RomeEngine.SystemDrawing
{
    public class CanvasFillStyle : CanvasStyleBase, ICanvasStyle
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
            var lastTransform = Graphics.Transform;

            Vector2 right = (Vector2)Transform.Column_0.normalized;
            Vector2 up = (Vector2)Transform.Column_1.normalized;
            Vector2 translate = (Vector2)Transform.Column_2;

            Graphics.Transform = new System.Drawing.Drawing2D.Matrix(right.x, right.y, up.x, up.y, translate.x, translate.y);

            a = Transform.MultiplyScale(a);
            b = Transform.MultiplyScale(b);

            var diff = (b - a);
            var length = diff.length;
            var dir = diff.normalized;

            Vector2 lineUp = new Vector2(-dir.y, dir.x);
            float globalWidthUnit = Transform.MultiplyVector(lineUp).length * 0.5f;
            widthA *= globalWidthUnit;
            widthB *= globalWidthUnit;

            var lineMatrix = Matrix3x3.New(dir, lineUp, a);

            nonAllocLinePoints[0] = lineMatrix.MultiplyPoint(new Vector2(0f, widthA));
            nonAllocLinePoints[1] = lineMatrix.MultiplyPoint(new Vector2(length, widthB));
            nonAllocLinePoints[2] = lineMatrix.MultiplyPoint(new Vector2(length, -widthB));
            nonAllocLinePoints[3] = lineMatrix.MultiplyPoint(new Vector2(0f, -widthA));
            Graphics.FillPolygon(Brush, nonAllocLinePoints);
            
            if (smoothEnding)
            {
                void DrawEndEllipse(Vector2 position, Vector2 size)
                {
                    var ellipsePos = lineMatrix.MultiplyPoint(position);
                    DrawEllipse(ellipsePos, size * 2f);
                }

                DrawEndEllipse(Vector2.zero, new Vector2(widthA, widthA));
                DrawEndEllipse(new Vector2(length, 0f), new Vector2(widthB, widthB));
            }
            Graphics.Transform = lastTransform;
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

        public void DrawText(string text, Rect rect, TextOptions options)
        {
            var format = StringFormat.GenericDefault;
            format.LineAlignment = (StringAlignment)(((int)options.Alignment >> 2) & 3);
            format.Alignment = (StringAlignment)((int)options.Alignment & 3);
            Graphics.DrawString(text, CanvasGraphics2D.CreateFont(options.FontSize), Brush, rect, format);
        }
    }
}
