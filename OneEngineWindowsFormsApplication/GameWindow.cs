using System;
using System.Windows.Forms;

using OneEngine;

namespace OneEngineWindowsFormsApplication
{
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
}
