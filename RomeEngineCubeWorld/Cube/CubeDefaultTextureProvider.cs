using RomeEngine;

namespace RomeEngineCubeWorld
{
    public sealed class CubeDefaultTextureProvider : ICubeTextureProvider
    {
        int width;
        int height;

        public CubeDefaultTextureProvider(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public Rect GetUvRect(int id)
        {
            int x = id % width;
            int y = id / width;
            Vector2 elementSize = new Vector2(1f / width, 1f / height);
            return Rect.FromLocationAndSize(elementSize * new Vector2(x, y), elementSize);
        }
    }
}
