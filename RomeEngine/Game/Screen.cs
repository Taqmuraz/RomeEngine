namespace RomeEngine
{
    public static class Screen
    {
        public static Vector2 Size => Engine.Instance.Runtime.SystemInfo.ScreenSize;
        public static Vector2 AspectVector
        {
            get
            {
                Vector2 size = Size;
                Vector2 ratio = Vector2.one;
                if (size.x > size.y) ratio.x = size.x / size.y;
                else ratio.y = size.y / size.x;
                return ratio;
            }
        }
        public static float AspectRatio
        {
            get
            {
                Vector2 size = Size;
                return size.x / size.y;
            }
        }
    }
}