namespace RomeEngine
{
    public interface ISystemInfo
    {
        Vector2 ScreenSize { get; }
        IMouseCursor Cursor { get; }
    }
}
