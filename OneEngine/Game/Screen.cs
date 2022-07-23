namespace OneEngine
{
    public static class Screen
    {
        public static Vector2 Size => Engine.Instance.Runtime.SystemInfo.ScreenSize;
        public static Vector2 AspectRatio
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
    }
}