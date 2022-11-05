using System.Drawing;

using RomeEngine;

namespace RomeEngine.SystemDrawing
{
    public class CanvasOutlineStyle : CanvasStyleBase, ICanvasStyle
    {
        public override void Setup()
        {
            Graphics.ResetTransform();
        }

        public void DrawPoint(Vector2 position, float radius)
        {
            Vector2 size = Transform.MultiplyVector(new Vector2(radius, radius) * 2f);
            position = Transform.MultiplyPoint(position);

            DrawEllipse(new Vector2(position.x - size.x * 0.5f, position.y - size.y * 0.5f), size);
        }

        public void DrawLine(Vector2 a, Vector2 b)
        {
            a = Transform.MultiplyPoint(a);
            b = Transform.MultiplyPoint(b);
            Graphics.DrawLine(Pen, a, b);
        }
        public void DrawLine(Vector2 a, Vector2 b, float widthA, float widthB, bool smoothEnding)
        {
            Vector2 right = (Vector2)Transform.Column_0.Normalized;
            Vector2 up = (Vector2)Transform.Column_1.Normalized;
            Vector2 translate = (Vector2)Transform.Column_2;

            Graphics.Transform = new System.Drawing.Drawing2D.Matrix(right.x, right.y, up.x, up.y, translate.x, translate.y);

            a = Transform.MultiplyScale(a);
            b = Transform.MultiplyScale(b);

            var nonAllocLinePoints = AllocPoints(4);
            Vector2 diff = (b - a);
            float length = diff.length;
            Vector2 dir = diff.normalized;

            Vector2 lineUp = new Vector2(-dir.y, dir.x);
            float globalWidthUnit = Transform.MultiplyVector(lineUp).length;
            widthA *= globalWidthUnit;
            widthB *= globalWidthUnit;

            Matrix3x3 lineMatrix = Matrix3x3.New(dir, lineUp, a);

            widthA *= 0.5f;
            widthB *= 0.5f;
            if (smoothEnding)
            {
                Graphics.DrawLine(Pen, lineMatrix.MultiplyPoint(new Vector2(0f, widthA)), lineMatrix.MultiplyPoint(new Vector2(length, widthB)));
                Graphics.DrawLine(Pen, lineMatrix.MultiplyPoint(new Vector2(length, -widthB)), lineMatrix.MultiplyPoint(new Vector2(0f, -widthA)));

                var hSize = new Vector2(widthA, widthA);
                Vector2 arcPos = (Vector2)lineMatrix.Column_2;
                Rect arcRect = new Rect(arcPos - hSize, arcPos + hSize);
                Graphics.DrawArc(Pen, arcRect, dir.ToAngle() + 90f, 180f);

                hSize = new Vector2(widthB, widthB);
                arcPos = lineMatrix.MultiplyPoint(new Vector2(length, 0f));
                arcRect = new Rect(arcPos - hSize, arcPos + hSize);
                Graphics.DrawArc(Pen, arcRect, dir.ToAngle() - 90f, 180f);
            }
            else
            {
                nonAllocLinePoints[0] = lineMatrix.MultiplyPoint(new Vector2(0f, widthA));
                nonAllocLinePoints[1] = lineMatrix.MultiplyPoint(new Vector2(length, widthB));
                nonAllocLinePoints[2] = lineMatrix.MultiplyPoint(new Vector2(length, -widthB));
                nonAllocLinePoints[3] = lineMatrix.MultiplyPoint(new Vector2(0f, -widthA));
                Graphics.DrawPolygon(Pen, nonAllocLinePoints);
            }
        }

        public void DrawEllipse(Vector2 center, Vector2 size)
        {
            center = Transform.MultiplyVector(center);
            size = Transform.MultiplyScale(size);
            Vector2 right = (Vector2)Transform.Column_0.Normalized;
            Vector2 up = (Vector2)Transform.Column_1.Normalized;
            Vector2 translate = (Vector2)Transform.Column_2;

            Graphics.Transform = new System.Drawing.Drawing2D.Matrix(right.x, right.y, up.x, up.y, translate.x, translate.y);
            Graphics.DrawEllipse(Pen, new RectangleF(center - size * 0.5f, size));
        }

        public void DrawRect(Rect rect)
        {
            Vector2 center = rect.Center;
            Vector2 size = rect.Size;
            center = Transform.MultiplyVector(center);
            size = Transform.MultiplyScale(size);
            Vector2 right = (Vector2)Transform.Column_0.Normalized;
            Vector2 up = (Vector2)Transform.Column_1.Normalized;
            Vector2 translate = (Vector2)Transform.Column_2;

            Graphics.Transform = new System.Drawing.Drawing2D.Matrix(right.x, right.y, up.x, up.y, translate.x, translate.y);
            Graphics.DrawRectangle(Pen, (Rectangle)Rect.FromCenterAndSize(center, size));
        }

        public void DrawPolygon(Vector2[] points)
        {
            var pointsF = AllocPoints(points.Length);
            for (int i = 0; i < points.Length; i++) pointsF[i] = Transform.MultiplyPoint(points[i]);
            Graphics.DrawPolygon(Pen, pointsF);
        }

        public void DrawText(string text, Rect rect, TextOptions options)
        {

        }
    }
}
