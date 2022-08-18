namespace OneEngine.UI
{
    public interface IHandlable
    {
        (Vector2 a, Vector2 b)[] GetHandleLines();
    }
}