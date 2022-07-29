namespace OneEngine
{
    public sealed class TextOptions
    {
        public static TextOptions Default { get; } = new TextOptions() { FontSize = 10f };

        public float FontSize { get; set; }
    }
}
