using RomeEngine;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AthensEnvironment
{
    sealed class AthensPanel : Panel, IEngineRuntine, ISystemInfo, IMouseCursor
    {
        IInputHandler inputHandler;
        RichTextBox logPanel;
        Vector2 lastMousePosition;

        public AthensPanel(Form parent)
        {
            logPanel = new SizeControlManager<RichTextBox>(parent, new RectangleF(0f, 0.6f, 1f, 0.4f)).Control;
            logPanel.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            
            logPanel.AppendText($"Initialized : {Time.DeltaTime}");
        }

        void IEngineRuntine.SetInputHandler(IInputHandler inputHandler)
        {
            this.inputHandler = inputHandler;
        }

        void IEngineRuntine.Log(string message)
        {
            logPanel.AppendText(message);
        }

        void IEngineRuntine.ShowFileOpenDialog(string root, string title, Action<string> callback)
        {
            RomeEngineStandardExplorerDialog.StandardFileDialog.ShowFileOpenDialog(root, title, callback);
        }

        void IEngineRuntine.ShowFileWriteDialog(string root, string fileName, string title, Action<string> callback)
        {
            RomeEngineStandardExplorerDialog.StandardFileDialog.ShowFileWriteDialog(root, fileName, title, callback);
        }

        ISystemInfo IEngineRuntine.SystemInfo => this;
        IFileSystem IEngineRuntine.FileSystem { get; } = new RomeEngineEditor.StandardFileSystem();

        void IEngineRuntine.Close()
        {
            ((Form)Parent).Close();
        }

        Vector2 ISystemInfo.ScreenSize => new Vector2(Width, Height);
        IMouseCursor ISystemInfo.Cursor => this;

        void IMouseCursor.SetPosition(Vector2 position)
        {
            Cursor.Position = (Point)position;
        }

        void IMouseCursor.SetVisible(bool visible)
        {
            if (visible) Cursor.Show();
            else Cursor.Hide();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            inputHandler.OnMouseMove(e.Location);
            inputHandler.OnMouseDelta(e.Location - lastMousePosition);
            lastMousePosition = e.Location;
        }

        void OnMouseButton(Vector2 position, MouseButtons buttons, Action<Vector2, int> callback)
        {
            if (buttons.HasFlag(MouseButtons.Left)) callback(position, 0);
            if (buttons.HasFlag(MouseButtons.Right)) callback(position, 1);
            if (buttons.HasFlag(MouseButtons.Middle)) callback(position, 2);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            OnMouseButton(e.Location, e.Button, inputHandler.OnMouseDown);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            OnMouseButton(e.Location, e.Button, inputHandler.OnMouseUp);
        }
        public void OnKeyDown(KeyCode key)
        {
            inputHandler.OnKeyDown(key);
        }
        public void OnKeyUp(KeyCode key)
        {
            inputHandler.OnKeyUp(key);
        }

        public bool IsRunning { get; set; } = true;
    }
}
