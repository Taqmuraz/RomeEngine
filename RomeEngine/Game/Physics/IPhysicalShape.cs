namespace RomeEngine
{
    public interface IPhysicalShape
    {
        PhysicalShapeType ShapeType { get; }
        Bounds Bounds { get; }
    }
}