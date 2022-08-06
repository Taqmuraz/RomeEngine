using OneEngine;
using OneEngine.UI;

namespace OneEngineGame
{
    public sealed class StringInputMenu : EditorMenu, IInputHandler
    {
        public string InputString { get; set; } = string.Empty;
        string header;

        bool shift;

        public StringInputMenu()
        {
            Engine.Instance.Runtime.SetInputHandler(this);
        }

        public StringInputMenu WithHeader(string header)
        {
            this.header = header;
            return this;
        }

        protected override void OnMenuClosed()
        {
            Engine.Instance.Runtime.SetInputHandler(new Input());
        }

        public override void Draw(EditorCanvas canvas)
        {
            var screenSize = Screen.Size;
            Vector2 menuSize = new Vector2(400f, 50f);
            var inputRect = new Rect((screenSize - menuSize) * 0.5f, (screenSize + menuSize) * 0.5f);
            canvas.DrawRect(inputRect, Color32.white);
            var textOptions = new TextOptions() { FontSize = 25f };

            if (string.IsNullOrEmpty(InputString) && !string.IsNullOrEmpty(header))
            {
                canvas.DrawText(header, inputRect, Color32.gray, textOptions);
            }
            canvas.DrawText(InputString == null ? string.Empty : InputString + "_", inputRect, Color32.black, textOptions);
        }

        public void OnKeyDown(KeyCode key)
        {
            if (key >= KeyCode.A && key <= KeyCode.Z)
            {
                char c = ((char)('a' + (key - KeyCode.A)));
                InputString += shift ? char.ToUpper(c) : c;
            }
            else if (key >= KeyCode.N0 && key <= KeyCode.N9)
            {
                char c = ((char)('0' + (key - KeyCode.N0)));
                InputString += c;
            }
            else if (key == KeyCode.Point)
            {
                InputString += '.';
            }
            else if (key == KeyCode.ShiftKey)
            {
                shift = true;
            }
            else if (!string.IsNullOrEmpty(InputString))
            {
                if (key == KeyCode.Backspace) InputString = InputString.Substring(0, InputString.Length - 1);
                else if (key == KeyCode.Enter)
                {
                    Close();
                }
            }
        }

        public void OnKeyUp(KeyCode key)
        {
            if (key == KeyCode.ShiftKey)
            {
                shift = false;
            }
        }

        public void OnMouseDown(Vector2 mousePosition, int button)
        {
        }

        public void OnMouseMove(Vector2 mousePosition)
        {
        }

        public void OnMouseUp(Vector2 mousePosition, int button)
        {
        }
    }
}