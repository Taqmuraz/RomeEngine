using RomeEngine;
using RomeEngine.UI;

namespace RomeEngineEditor
{
    public sealed class EditorCanvas : Canvas
    {
        public Color32 RectColor { get; set; } = Color32.white * 0.6f;
        public Color32 ButtonColor { get; set; } = Color32.gray;
        public Color32 TextColor { get; set; } = Color32.black;
        public Color32 ButtonTextColor { get; set; } = Color32.white;
        public Color32 ButtonHoveredColor { get; set; } = Color32.blue;
        public Color32 ButtonPressedColor { get; set; } = Color32.blue * 0.5f;

        public bool DrawButton(string text, Rect rect, TextOptions textOptions)
        {
            return DrawButton(text, rect, ButtonTextColor, ButtonColor, ButtonHoveredColor, ButtonPressedColor, textOptions);
        }
        public void DrawText(string text, Rect rect, TextOptions textOptions)
        {
            DrawText(text, rect, TextColor, textOptions);
        }
        public void DrawRect(Rect rect)
        {
            DrawRect(rect, RectColor);
        }
        public void DrawRectWithText(string text, Rect rect, TextOptions textOptions)
        {
            DrawRect(rect);
            DrawText(text, rect, textOptions);
        }
    }
}