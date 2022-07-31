using OneEngine;
using OneEngine.UI;

namespace OneEngineGame
{
    public sealed class StringInputMenu : EditorMenu
    {
        public string InputString { get; set; } = string.Empty;
        public bool Done { get; private set; } = false;

        public override void Draw(Canvas canvas)
        {
            var screenSize = Screen.Size;
            Vector2 menuSize = new Vector2(400f, 200f);
            var inputRect = new Rect((screenSize - menuSize) * 0.5f, (screenSize + menuSize) * 0.5f);
            canvas.DrawRect(inputRect, Color32.white);
            canvas.DrawText(InputString + "_", inputRect, Color32.black, TextOptions.Default);

            for (int i = 0; i < ('z' - 'a'); i++)
            {
                if (Input.GetKeyDown(KeyCode.A + i))
                {
                    char c = ((char)('a' + i));
                    InputString += Input.GetKey(KeyCode.ShiftKey) ? char.ToUpper(c) : c;
                }
            }
            if (Input.GetKey(KeyCode.Backspace)) InputString = InputString.Substring(0, InputString.Length - 1);
            if (Input.GetKeyDown(KeyCode.Enter))
            {
                Done = true;
                Close();
            }
        }
    }
}