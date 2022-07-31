using System;
using System.IO;
using System.Windows.Forms;

using OneEngine;

namespace OneEngineWindowsFormsApplication
{
    class GameCanvas : Panel, IEngineRuntine, ISystemInfo
    {
        TextWriter log;
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
            log.WriteLine(message);
        }

        public GameCanvas(TextWriter log)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);

            this.log = log;
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
}
