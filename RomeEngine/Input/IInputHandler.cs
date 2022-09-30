namespace RomeEngine
{
    public interface IInputHandler
    {
        void OnKeyDown(KeyCode key);
        void OnKeyUp(KeyCode key);
        void OnMouseDown(Vector2 mousePosition, int button);
        void OnMouseMove(Vector2 mousePosition);
        void OnMouseDelta(Vector2 delta);
        void OnMouseUp(Vector2 mousePosition, int button);
    }
}
