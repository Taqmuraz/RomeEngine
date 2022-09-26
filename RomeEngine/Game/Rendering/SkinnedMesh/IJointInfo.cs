namespace RomeEngine
{
    public interface IJointInfo
    {
        Transform Transform { get; }
        Matrix4x4 InitialState { get; }
    }
}
