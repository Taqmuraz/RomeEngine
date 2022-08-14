using System;
using System.IO;
using System.Windows.Forms;

using OneEngine;

namespace OneEngineWindowsFormsApplication
{
    class GameWindow : Form
    {
        GameCanvas canvas;

        public GameWindow(TextWriter log)
        {
            canvas = new GameCanvas(log);
            canvas.Parent = this;

            SetBounds(0, 0, 1280, 640);
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
