namespace OneEngine
{
    public sealed class TextOptions
    {
        public static TextOptions Default { get; } = new TextOptions() { FontSize = 14f };

        public float FontSize { get; set; }
        public TextAlignment Alignment { get; set; } = TextAlignment.MiddleCenter;
    }
}
