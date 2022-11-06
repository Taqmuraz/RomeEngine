using RomeEngine;
using System;
using System.Windows.Forms;

namespace AthensEnvironment
{
    sealed class AthensWindow : Form
    {
        AthensPanel panel;

        public AthensWindow()
        {
            WindowState = FormWindowState.Maximized;
            panel = new AthensPanel(this);
            panel.Parent = this;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            panel.OnKeyDown((KeyCode)(int)e.KeyCode);
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            panel.OnKeyUp((KeyCode)(int)e.KeyCode);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            panel.IsRunning = false;
        }
    }
}
