using OpenTK.Graphics.OpenGL;
using RomeEngine;

namespace RomeEngineOpenGL
{
    class OpenGLGraphics2D : IGraphics2D
    {
        public Matrix3x3 Transform { get; set; } = Matrix3x3.identity;
        public IGraphicsBrush Brush { get; set; }
        public IGraphicsStyle Style { get; set; }
        IStyle2D outlineStyle = new OutlineStyle2D();
        IStyle2D fillStyle = new FillStyle2D();
        public IGraphicsStyle OutlineStyle => outlineStyle;
        public IGraphicsStyle FillStyle => fillStyle;

        int width, height;
        public void Setup(int width, int height)
        {
            this.width = width;
            this.height = height;
            GL.LoadIdentity();
            GL.Viewport(0, 0, width, height);
            outlineStyle.Setup();
            fillStyle.Setup();
        }

        IStyle2D SetupStyle()
        {
            if (Style is IStyle2D style2D)
            {
                style2D.Brush = Brush;
                style2D.Transform = Transform * Matrix3x3.Viewport(width, height).GetInversed();
                return style2D;
            }
            else
            {
                throw new System.InvalidOperationException("Style must implement IStyle2D interface");
            }
        }

        public void Clear(Color32 color)
        {

        }

        public void DrawPoint(Vector2 position, float radius)
        {
            
        }

        public void DrawLine(Vector2 a, Vector2 b)
        {
            SetupStyle().DrawLine(a, b, 1f, 1f, false);
        }

        public void DrawLine(Vector2 a, Vector2 b, float widthA, float widthB, bool smoothEnding)
        {
            SetupStyle().DrawLine(a, b, widthA, widthB, smoothEnding);
        }

        public void DrawText(string text, Rect rect, TextOptions options)
        {
            SetupStyle().DrawText(text, rect, options);
        }

        public void DrawEllipse(Vector2 center, Vector2 size)
        {
            SetupStyle().DrawEllipse(center, size);
        }

        public void DrawRect(Rect rect)
        {
            SetupStyle().DrawRect(rect);
        }

        public void DrawPolygon(Vector2[] points)
        {
            SetupStyle().DrawPolygon(points);
        }

        public Vector2 MeasureText(string text, float fontSize)
        {
            return new Vector2();
        }
    }
}
