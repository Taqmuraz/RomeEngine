using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using RomeEngine;
using RomeEngineGame;
using System;
using System.IO;
using System.Windows.Forms;

namespace RomeEngineOpenGL
{
    class MainWindow : GameWindow, IEngineRuntine, ISystemInfo
    {
        TextWriter logStream;
        IInputHandler inputHandler;
        IEngine engine;
        IGraphicsContext context;
        OpenGLGraphics graphics;
        OpenGLGraphics2D graphics2D;

        public MainWindow(int width, int height, TextWriter logStream)
        {
            Width = width;
            Height = height;
            Title = "RomeEngine OpenGL";
            this.logStream = logStream;

            context = new OpenGLContext();

            engine = RomeEngineGame.RomeEngineGame.StartGame(this);
            graphics = new OpenGLGraphics();
            graphics2D = new OpenGLGraphics2D();
            context = new OpenGLContext();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            engine.UpdateGameState();

            graphics.Setup(Width, Height);
            engine.UpdateGraphics3D(graphics, context);
            graphics2D.Setup(Width, Height);
            engine.UpdateGraphics2D(graphics2D);
            
            /*GL.ClearColor(Color32.gray);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            graphics2D.Setup(Width, Height);
            graphics2D.Brush = new SingleColorBrush(Color32.red);
            graphics2D.Style = graphics2D.FillStyle;
            graphics2D.DrawRect(new Rect(0f, 0f, 150f, 200f));
            graphics2D.DrawRect(new Rect(150f, 0f, 150f, 50f));
            graphics2D.DrawRect(new Rect(Width * 0.5f - 100, 387.5f, 200, 30));

            /*
            GL.ClearColor(Color32.gray);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Viewport(0, 0, Width, Height);
            GL.Begin(PrimitiveType.Triangles);
            GL.Color4(Color32.red);
            GL.Vertex2(0f, 0.5f);
            GL.Vertex2(0.5f, 0.5f);
            float t = Time.CurrentTime * 10f;
            GL.Vertex2(Mathf.Cos(t), Mathf.Sin(t));
            GL.End();
            */

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

        public ISystemInfo SystemInfo => this;
        public IFileSystem FileSystem { get; } = new StandardFileSystem();
        public RomeEngine.Vector2 ScreenSize => new RomeEngine.Vector2(Width, Height);
    }
}
