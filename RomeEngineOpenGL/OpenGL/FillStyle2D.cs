using OpenTK.Graphics.OpenGL;
using RomeEngine;

namespace RomeEngineOpenGL
{
    class FillStyle2D : Style2D, IStyle2D
    {
        public FillStyle2D(IGraphicsContext context) : base(context)
        {
        }

        public Matrix3x3 Transform { get; set; }
        public IGraphicsBrush Brush { get; set; }

        public void DrawEllipse(Vector2 center, Vector2 size)
        {
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color4(Brush.Color);
            for (int i = 0; i < 36; i++)
            {
                float angle = i * 10f;
                Vector2 vertex = Transform.MultiplyPoint(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
                GL.Vertex3(vertex.x, vertex.y, Depth);
            }
            NextDepth();
            GL.End();
        }

        public void DrawLine(Vector2 a, Vector2 b, float widthA, float widthB, bool smoothEnding)
        {
            Matrix3x3 lineMatrix = Transform * Matrix3x3.WorldTransform(b - a, Vector2.Cross(b - a).normalized, a);
            GL.Begin(PrimitiveType.Quads);
            GL.Color4(Brush.Color);
            widthA *= 0.5f;
            widthB *= 0.5f;
            MatrixVertex(new Vector2(0f, widthA), ref lineMatrix);
            MatrixVertex(new Vector2(1f, widthB), ref lineMatrix);
            MatrixVertex(new Vector2(1f, -widthB), ref lineMatrix);
            MatrixVertex(new Vector2(0f, -widthB), ref lineMatrix);
            NextDepth();
            GL.End();
        }

        public void DrawPolygon(Vector2[] points)
        {
            GL.Begin(PrimitiveType.Polygon);
            GL.Color4(Brush.Color);
            for (int i = 1; i < points.Length; i++)
            {
                Vector2 point = Transform.MultiplyPoint(points[i]);
                GL.Vertex3(point.x, point.y, Depth);
            }
            NextDepth();
            GL.End();
        }

        public void DrawRect(Rect rect)
        {
            GL.Begin(PrimitiveType.Quads);
            rect = Rect.FromCenterAndSize(Transform.MultiplyPoint(rect.Center), Transform.MultiplyVector(rect.Size));
            GL.Color4(Brush.Color);
            GL.Vertex3(rect.min.x, rect.min.y, Depth);
            GL.Vertex3(rect.min.x, rect.max.y, Depth);
            GL.Vertex3(rect.max.x, rect.max.y, Depth);
            GL.Vertex3(rect.max.x, rect.min.y, Depth);
            NextDepth();
            GL.End();
        }
    }
}
