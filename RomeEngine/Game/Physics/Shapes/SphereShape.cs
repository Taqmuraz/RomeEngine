namespace RomeEngine
{
    public sealed class SphereShape : IPhysicalShape
    {
        public Vector3 Center { get; set; }
        public float Radius { get; set; }
        public PhysicalShapeType ShapeType => PhysicalShapeType.Sphere;

        public Bounds Bounds => new Bounds(Center, new Vector3(Radius, Radius, Radius) * 2f);
    }
}