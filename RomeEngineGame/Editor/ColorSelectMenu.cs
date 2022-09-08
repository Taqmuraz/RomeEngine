using RomeEngine;

namespace OneEngineGame
{
    public sealed class ColorSelectMenu : EditorMenu
    {
        public Color32 Color { get; set; } = Color32.white;
        static Color32 bufferColor = Color32.white;

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
            byte a = (byte)canvas.DrawScrollbar(hash + 3, 0f, 255f, Color.a, Rect.FromLocationAndSize(menuStart + new Vector2(0f, elementHeight * 3f), new Vector2(menuSize.x, elementHeight)), 0, Color32.black);

            Color = new Color32(r, g, b, a);

            if (canvas.DrawButton("OK", Rect.FromLocationAndSize(menuStart + new Vector2(0f, elementHeight * 4f), new Vector2(menuSize.x, elementHeight)), TextOptions.Default))
            {
                Close();
            }
            if (canvas.DrawButton("Copy color", Rect.FromLocationAndSize(menuStart + new Vector2(0f, elementHeight * 5f), new Vector2(menuSize.x, elementHeight)), TextOptions.Default))
            {
                bufferColor = Color;
                Close();
            }
            if (canvas.DrawButton("Paste color", Rect.FromLocationAndSize(menuStart + new Vector2(0f, elementHeight * 6f), new Vector2(menuSize.x, elementHeight)), TextOptions.Default))
            {
                Color = bufferColor;
                Close();
            }
        }
    }
}