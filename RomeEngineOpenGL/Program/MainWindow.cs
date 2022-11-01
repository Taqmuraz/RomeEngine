using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using RomeEngine;
using RomeEngineEditor;
using System;
using System.IO;

namespace RomeEngineOpenGL
{
    class MainWindow : GameWindow, IEngineRuntine, ISystemInfo, IMouseCursor
    {
        TextWriter logStream;
        IInputHandler inputHandler;
        IEngine engine;
        IGraphicsContext context;
        OpenGLGraphics graphics;
        OpenGLGraphics2D graphics2D;
        RomeEngine.Vector2? desiredMousePosition;

        public MainWindow(int width, int height, TextWriter logStream)
        {
            Location = new System.Drawing.Point();
            Width = width;
            Height = height;
            Title = "RomeEngine OpenGL";
            this.logStream = logStream;

            context = new OpenGLContext();

            engine = RomeEngineEditor.RomeEngineGame.StartGame(this);
            graphics = new OpenGLGraphics();
            context = new OpenGLContext();
            graphics2D = new OpenGLGraphics2D(context);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            engine.UpdateGameState();

            graphics.Setup(Width, Height);
            engine.UpdateGraphics3D(graphics, context);
            graphics.RenderScene();
            graphics2D.Setup(Width, Height);
            engine.UpdateGraphics2D(graphics2D);
            graphics2D.RenderScene();

            SwapBuffers();
        }

        int FromMouseToKey(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.Left:return 0;
                case MouseButton.Middle:return 2;
                case MouseButton.Right: return 1;
                default:
                    return 0;
            }
        }
        KeyCode FromKeyToCode(Key key)
        {
            switch (key)
            {
                default:return KeyCode.None;
                case Key.ShiftRight:
                case Key.ShiftLeft:return KeyCode.ShiftKey;
                case Key.BackSpace:return KeyCode.Backspace;
                case Key.Space:return KeyCode.Space;
                case Key.KeypadEnter:
                case Key.Enter: return KeyCode.Enter;
                case Key.Escape: return KeyCode.Escape;
                case Key k when k >= Key.A && k <= Key.Z:return KeyCode.A + (k - Key.A);
                case Key k when k >= Key.Number0 && k <= Key.Number9:return KeyCode.N0 + (k - Key.Number0);
                case Key k when k >= Key.Keypad0 && k <= Key.Keypad9: return KeyCode.N0 + (k - Key.Keypad0);
                case Key.Period:return KeyCode.Point;
                case Key.Minus:
                case Key.KeypadMinus: return KeyCode.Minus;
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            inputHandler.OnMouseDown(e.Position, FromMouseToKey(e.Button));
        }
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            inputHandler.OnMouseUp(e.Position, FromMouseToKey(e.Button));
        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            inputHandler.OnMouseMove(e.Position);
            inputHandler.OnMouseDelta(new RomeEngine.Vector2(e.XDelta, e.YDelta));
            if (desiredMousePosition.HasValue)
            {
                Mouse.SetPosition(desiredMousePosition.Value.x, desiredMousePosition.Value.y);
                desiredMousePosition = null;
            }
        }
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            inputHandler.OnKeyDown(FromKeyToCode(e.Key));
        }
        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            inputHandler.OnKeyUp(FromKeyToCode(e.Key));
        }

        public void SetInputHandler(IInputHandler inputHandler)
        {
            this.inputHandler = inputHandler;
        }

        public void Log(string message)
        {
            logStream.WriteLine(message);
        }

        public void ShowFileOpenDialog(string root, string title, Action<string> callback)
        {
            RomeEngineStandardExplorerDialog.StandardFileDialog.ShowFileOpenDialog(root, title, callback);
        }

        public void ShowFileWriteDialog(string root, string fileName, string title, Action<string> callback)
        {
            RomeEngineStandardExplorerDialog.StandardFileDialog.ShowFileWriteDialog(root, fileName, title, callback);
        }

        public ISystemInfo SystemInfo => this;
        public IFileSystem FileSystem { get; } = new StandardFileSystem();
        public RomeEngine.Vector2 ScreenSize => new RomeEngine.Vector2(Width, Height);

        void IMouseCursor.SetPosition(RomeEngine.Vector2 position)
        {
            desiredMousePosition = position;
        }

        void IMouseCursor.SetVisible(bool visible)
        {
            CursorVisible = visible;
        }

        IMouseCursor ISystemInfo.Cursor => this;

        void IEngineRuntine.Close()
        {
            Close();
        }
    }
}
