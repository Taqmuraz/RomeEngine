using System;
using System.IO;
using System.Windows.Forms;

using RomeEngine;

namespace RomeEngineWindowsFormsApplication
{
    class GameWindow : Form
    {
        GameCanvas canvas;

        public GameWindow(TextWriter log)
        {
            int width = 1280;
            int height = 640;

            canvas = new GameCanvas(log, width, height);
            canvas.Parent = this;

            SetBounds(0, 0, width, height);
        }

        protected override void OnResize(EventArgs e)
        {
            canvas.Size = new System.Drawing.Size(Width - 50, Height - 50);
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
}
