namespace RomeEngine
{
    public interface IColliderShape
    {
        ColliderShapeType ShapeType { get; }
        Bounds Bounds { get; }
    }
}