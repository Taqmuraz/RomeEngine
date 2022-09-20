using RomeEngine;
using System;

namespace RomeEngineGame
{
    public sealed class DropdownMenu : EditorMenu
    {
        public string[] DropdownOptions { get; set; }
        public int SelectedOption { get; private set; }

        float elementWidth = 200f;
        float elementHeight = 30f;
        int maxElements = 10;
        int scroll = 0;

        public DropdownMenu()
        {
            Vector2 screenSize = Screen.Size;
            Rect = new Rect(screenSize.x * 0.5f - elementWidth * 0.5f, screenSize.y * 0.5f - elementHeight * maxElements * 0.5f, elementWidth, elementHeight * maxElements);
        }

        public override void Draw(EditorCanvas canvas)
        {
            Rect startRect = Rect;

            int diff = DropdownOptions.Length - maxElements;
            if (diff > 0)
            {
                scroll = (int)canvas.DrawScrollbar(GetHashCode(), 0f, diff, scroll, new Rect(Rect.X + elementWidth, Rect.Y, 30f, Rect.Height), 1, Color32.gray);
            } else
            {
                scroll = 0;
            }

            for (int i = scroll; i < Math.Min(DropdownOptions.Length, maxElements + scroll); i++)
            {
                if (canvas.DrawButton(DropdownOptions[i], new Rect(startRect.X, startRect.Y + elementHeight * (i - scroll), elementWidth, elementHeight), TextOptions.Default))
                {
                    SelectedOption = i;
                    Close();
                    break;
                }
            }
        }
    }
}