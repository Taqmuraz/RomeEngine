
using RomeEngine;

namespace OneEngineWindowsFormsApplication
{
    internal class CanvasBrush : IGraphicsBrush
    {
        public CanvasBrush(Color32 color, int size)
        {
            Color = color;
            Size = size;
        }

        public static CanvasBrush Default { get; } = new CanvasBrush(Color32.black, 5);

        public Color32 Color { get; }
        public int Size { get; }
    }
}
