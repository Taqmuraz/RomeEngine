using System;
using System.Drawing;

using RomeEngine;

namespace OneEngineWindowsFormsApplication
{
    class CanvasGraphics : IGraphics2D
    {
        public Matrix3x3 Transform
        {
            get => transform;
            set
            {
                transform = value;
                try
                {
                    Graphics.Transform = new System.Drawing.Drawing2D.Matrix(value.Column_0.x, value.Column_0.y, value.Column_1.x, value.Column_1.y, value.Column_2.x, value.Column_2.y);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
        }
        Matrix3x3 transform = Matrix3x3.identity;

        public IGraphicsBrush Brush { get; set; } = CanvasBrush.Default;
        public IGraphicsStyle Style
        {
            get => style;
            set
            {
                if (value is ICanvasStyle canvasStyle) style = canvasStyle;
                else throw new ArgumentException("Style must implement the ICanvasStyle interface");
            }
        }
        public IGraphicsStyle OutlineStyle => drawStyle;
        public IGraphicsStyle FillStyle => fillStyle;
        ICanvasStyle drawStyle;
        ICanvasStyle fillStyle;
        ICanvasStyle style;

        public Graphics Graphics { get; set; }
        public Vector2 ScreenSize { get; set; }

        public CanvasGraphics()
        {
            drawStyle = new CanvasOutlineStyle();
            fillStyle = new CanvasFillStyle();
            style = drawStyle;
        }

        ICanvasStyle SetupStyle()
        {
            style.Brush = new SolidBrush(Brush.Color);
            style.Pen = new Pen(Brush.Color, Brush.Size);
            style.Graphics = Graphics;
            style.Transform = Transform;
            style.Setup();
            return style;
        }

        public void DrawPoint(Vector2 position, float radius)
        {
            SetupStyle().DrawPoint(position, radius);
        }

        public void DrawLine(Vector2 a, Vector2 b)
        {
            SetupStyle().DrawLine(a, b);
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
            return Graphics.MeasureString(text, CreateFont(fontSize));
        }

        public static Font CreateFont(float fontSize)
        {
            return new Font(SystemFonts.DefaultFont.FontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
        }

        public void Clear(Color32 color)
        {
            Graphics.Clear(color);
        }
    }
}
