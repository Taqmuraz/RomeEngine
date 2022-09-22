﻿using System;
using System.IO;
using System.Windows.Forms;

using RomeEngine;

namespace OneEngineWindowsFormsApplication
{
    class GameCanvas : Panel, IEngineRuntine, ISystemInfo
    {
        TextWriter log;
        IEngine engine;
        CanvasGraphics2D graphics2D = new CanvasGraphics2D();
        CanvasGraphics3D graphics3D = new CanvasGraphics3D();
        GraphicsContext context = new GraphicsContext();

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

        public GameCanvas(TextWriter log, int width, int height)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            SetBounds(0, 0, width, height);

            this.log = log;
            engine = RomeEngineGame.RomeEngineGame.StartGame(this);

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
            graphics2D.Graphics = e.Graphics;
            graphics3D.SetGraphics(e.Graphics, context);
            graphics2D.ScreenSize = Size;
            engine.UpdateGraphics3D(graphics3D, context);
            graphics3D.End();
            engine.UpdateGraphics2D(graphics2D);
        }

        public void ShowFileOpenDialog(string root, string title, Action<string> callback)
        {
            var thread = new System.Threading.Thread(() =>
            {
                var dialog = new OpenFileDialog();
                dialog.Title = title;
                dialog.InitialDirectory = Path.GetFullPath(root);
                dialog.Multiselect = false;
                dialog.FileOk += (s, e) => callback(dialog.FileName);
                dialog.ShowDialog();
            });
            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
        }

        public void ShowFileWriteDialog(string root, string fileName, string title, Action<string> callback)
        {
            var thread = new System.Threading.Thread(() =>
            {
                var dialog = new SaveFileDialog();
                dialog.Title = title;
                dialog.InitialDirectory = Path.GetFullPath(root);
                dialog.FileName = fileName;
                dialog.FileOk += (s, e) => callback(dialog.FileName);
                dialog.ShowDialog();
            });
            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
        }

        public IFileSystem FileSystem { get; } = new FileSystem();
    }
}
