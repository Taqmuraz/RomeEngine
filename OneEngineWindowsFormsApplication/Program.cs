using System;
using System.Windows.Forms;
using System.Drawing;

using OneEngine;
using System.Linq;
using OneEngineGame;

namespace OneEngineWindowsFormsApplication
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameWindow());
        }
    }
    class GameWindow : Form
    {
        GameCanvas canvas;

        public GameWindow()
        {
            canvas = new GameCanvas();
            canvas.Parent = this;

            SetBounds(0, 0, 1024, 800);
        }

        protected override void OnResize(EventArgs e)
        {
            canvas.Size = Size;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            canvas.InputHandler.OnKeyDown((KeyCode)(int)e.KeyCode);
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            canvas.InputHandler.OnKeyUp((KeyCode)(int)e.KeyCode);
        }
    }
    class GameCanvas : Panel, IEngineRuntine, ISystemInfo
    {
        IEngine engine;
        CanvasGraphics graphics = new CanvasGraphics();

        public ISystemInfo SystemInfo => this;
        public Vector2 ScreenSize => new Vector2(Width, Height);
        public IInputHandler InputHandler { get; private set; }

        public void SetInputHandler(IInputHandler inputHandler)
        {
            InputHandler = inputHandler;
        }

        public void Log(string message)
        {

        }

        public GameCanvas()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);

            engine = OneEngineGame.OneEngineGame.StartGame(this);

            Timer timer = new Timer();
            timer.Tick += (s, e) => UpdateEngine();
            timer.Interval = 20;
            timer.Start();
        }

        void UpdateEngine()
        {
            engine.UpdateGameState();
            Refresh();
        }

        void OnMouseButton(Vector2 position, MouseButtons buttons, Action<Vector2, int> callback)
        {
            if (buttons.HasFlag(MouseButtons.Left)) callback(position, 0);
            if (buttons.HasFlag(MouseButtons.Right)) callback(position, 1);
            if (buttons.HasFlag(MouseButtons.Middle)) callback(position, 2);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            OnMouseButton(e.Location, e.Button, InputHandler.OnMouseDown);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            OnMouseButton(e.Location, e.Button, InputHandler.OnMouseUp);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            OnMouseButton(e.Location, MouseButtons.Left, (pos, button) => InputHandler.OnMouseMove(pos));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            graphics.Graphics = e.Graphics;
            graphics.ScreenSize = Size;
            engine.UpdateGraphics(graphics);
        }
    }
    class CanvasBrush : IGraphicsBrush
    {
        public CanvasBrush(Color32 color, int size)
        {
            Color = color;
            Size = size;
        }

        public static CanvasBrush Default { get; } = new CanvasBrush(Color32.black, 5);

        public Color32 Color { get; }
        public int Size { get; }
    }
    interface ICanvasStyle : IGraphicsStyle
    {
        void DrawPoint(Vector2 position, float radius);
        void DrawLine(Vector2 a, Vector2 b);
        void DrawLine(Vector2 a, Vector2 b, float widthA, float widthB);
        void DrawText(Vector2 position, string text, int fontSize);
        void DrawEllipse(Vector2 center, Vector2 size);
        void DrawRect(Rect rect);
        void DrawPolygon(Vector2[] points);

        Graphics Graphics { get; set; }
        SolidBrush Brush { get; set; }
        Pen Pen { get; set; }
    }

    class CanvasStyleBase
    {
        public Graphics Graphics { get; set; }
        public SolidBrush Brush { get; set; }
        public Pen Pen { get; set; }

        static PointF[][] nonAllocPoints;

        protected PointF[] AllocPoints(int count)
        {
            return nonAllocPoints[count];
        }

        static CanvasStyleBase()
        {
            nonAllocPoints = new PointF[128][];
            for (int i = 0; i < nonAllocPoints.Length; i++) nonAllocPoints[i] = new PointF[i];
        }

        public void DrawText(Vector2 position, string text, int fontSize)
        {
            DrawInReversedScale(() => Graphics.DrawString(text, CanvasGraphics.CreateFont(fontSize), Brush, position));
        }
        protected void DrawInReversedScale(Action drawAction)
        {
            var transform = Graphics.Transform;
            var reversed = transform.Clone();
            reversed.Scale(1f, -1f);
            Graphics.Transform = reversed;
            drawAction();
            Graphics.Transform = transform;
        }
    }

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
        public void DrawLine(Vector2 a, Vector2 b, float widthA, float widthB)
        {
            var nonAllocLinePoints = AllocPoints(4);
            Vector3 diff = (b - a);
            float length = diff.length;
            Vector3 dir = diff.normalized;
            Matrix3x3 lineMatrix = Matrix3x3.New(dir, new Vector3(-dir.y, dir.x, 0f), a);
            widthA *= 0.5f;
            widthB *= 0.5f;
            nonAllocLinePoints[0] = lineMatrix.MultiplyPoint(new Vector2(0f, widthA));
            nonAllocLinePoints[1] = lineMatrix.MultiplyPoint(new Vector2(length, widthB));
            nonAllocLinePoints[2] = lineMatrix.MultiplyPoint(new Vector2(length, -widthB));
            nonAllocLinePoints[3] = lineMatrix.MultiplyPoint(new Vector2(0f, -widthA));
            Graphics.FillPolygon(Brush, nonAllocLinePoints);
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
    }

    class CanvasDrawStyle : CanvasStyleBase, ICanvasStyle
    {
        public void DrawPoint(Vector2 position, float radius)
        {
            DrawEllipse(new Vector2(position.x - radius, position.y - radius), new Vector2(radius * 2f, radius * 2f));
        }

        public void DrawLine(Vector2 a, Vector2 b)
        {
            Graphics.DrawLine(Pen, a, b);
        }
        public void DrawLine(Vector2 a, Vector2 b, float widthA, float widthB)
        {
            var nonAllocLinePoints = AllocPoints(4);
            Vector3 diff = (b - a);
            float length = diff.length;
            Vector3 dir = diff.normalized;
            Matrix3x3 lineMatrix = Matrix3x3.New(dir, new Vector3(-dir.y, dir.x, 0f), a);
            widthA *= 0.5f;
            widthB *= 0.5f;
            nonAllocLinePoints[0] = lineMatrix.MultiplyPoint(new Vector2(0f, widthA));
            nonAllocLinePoints[1] = lineMatrix.MultiplyPoint(new Vector2(length, widthB));
            nonAllocLinePoints[2] = lineMatrix.MultiplyPoint(new Vector2(length, -widthB));
            nonAllocLinePoints[3] = lineMatrix.MultiplyPoint(new Vector2(0f, -widthA));
            Graphics.DrawPolygon(Pen, nonAllocLinePoints);
        }

        public void DrawEllipse(Vector2 center, Vector2 size)
        {
            DrawInReversedScale(() => Graphics.DrawEllipse(Pen, new RectangleF(center - size * 0.5f, size)));
        }

        public void DrawRect(Rect rect)
        {
            Graphics.DrawRectangle(Pen, (Rectangle)rect);
        }

        public void DrawPolygon(Vector2[] points)
        {
            var pointsF = AllocPoints(points.Length);
            for (int i = 0; i < points.Length; i++) pointsF[i] = points[i];
            Graphics.DrawPolygon(Pen, pointsF);
        }
    }

    class CanvasGraphics : IGraphics
    {
        public Matrix3x3 Transform
        {
            get => transform;
            set
            {
                transform = value;
                Graphics.Transform = new System.Drawing.Drawing2D.Matrix(value.Column_0.x, value.Column_0.y, value.Column_1.x, value.Column_1.y, value.Column_2.x, value.Column_2.y);
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
        public IGraphicsStyle DrawStyle => drawStyle;
        public IGraphicsStyle FillStyle => fillStyle;
        ICanvasStyle drawStyle;
        ICanvasStyle fillStyle;
        ICanvasStyle style;

        public Graphics Graphics { get; set; }
        public Vector2 ScreenSize { get; set; }

        public CanvasGraphics()
        {
            drawStyle = new CanvasDrawStyle();
            fillStyle = new CanvasFillStyle();
            style = drawStyle;
        }

        ICanvasStyle SetupStyle()
        {
            style.Brush = new SolidBrush(Brush.Color);
            style.Pen = new Pen(Brush.Color, Brush.Size);
            style.Graphics = Graphics;
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

        public void DrawLine(Vector2 a, Vector2 b, float widthA, float widthB)
        {
            SetupStyle().DrawLine(a, b, widthA, widthB);
        }

        public void DrawText(Vector2 position, string text, int fontSize)
        {
            SetupStyle().DrawText(position, text, fontSize);
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

        public Vector2 MeasureText(string text, int fontSize)
        {
            return Graphics.MeasureString(text, CreateFont(fontSize));
        }

        public static Font CreateFont(int fontSize)
        {
            return new Font(SystemFonts.DefaultFont.FontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
        }

        public void Clear(Color32 color)
        {
            Graphics.Clear(color);
        }
    }
}
