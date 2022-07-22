namespace OneEngine
{
    public sealed class SingleColorBrush : IGraphicsBrush
    {
        public SingleColorBrush(Color32 color, int size = 5)
        {
            Color = color;
            Size = size;
        }

        public Color32 Color { get; }
        public int Size { get; }
    }
}
