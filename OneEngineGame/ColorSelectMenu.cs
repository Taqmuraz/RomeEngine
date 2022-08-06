using OneEngine;

namespace OneEngineGame
{
    public sealed class ColorSelectMenu : EditorMenu
    {
        public Color32 Color { get; set; } = Color32.white;

        public override void Draw(EditorCanvas canvas)
        {
            Vector2 screenSize = Screen.Size;
            Vector2 menuSize = new Vector2(400f, 300f);
            Vector2 menuCenter = screenSize * 0.5f;
            Vector2 menuStart = (screenSize - menuSize) * 0.5f;
            float elementHeight = 25f;
            canvas.DrawRect(Rect.FromCenterAndSize(menuCenter, menuSize), Color);
            int hash = GetHashCode();
            byte r = (byte)canvas.DrawScrollbar(hash, 0f, 255f, Color.r, Rect.FromLocationAndSize(menuStart, new Vector2(menuSize.x, elementHeight)), 0, Color32.red);
            byte g = (byte)canvas.DrawScrollbar(hash + 1, 0f, 255f, Color.g, Rect.FromLocationAndSize(menuStart + new Vector2(0f, elementHeight), new Vector2(menuSize.x, elementHeight)), 0, Color32.green);
            byte b = (byte)canvas.DrawScrollbar(hash + 2, 0f, 255f, Color.b, Rect.FromLocationAndSize(menuStart + new Vector2(0f, elementHeight * 2f), new Vector2(menuSize.x, elementHeight)), 0, Color32.blue);

            Color = new Color32(r, g, b, Color.a);

            if (canvas.DrawButton("OK", Rect.FromLocationAndSize(menuStart + new Vector2(0f, elementHeight * 3f), new Vector2(menuSize.x, elementHeight)), TextOptions.Default))
            {
                Close();
            }
        }
    }
}