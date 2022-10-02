namespace RomeEngine
{
    public sealed class SphereShape : IColliderShape
    {
        public Vector3 Center { get; set; }
        public float Radius { get; set; }
        public ColliderShapeType ShapeType => ColliderShapeType.Sphere;

        public Bounds Bounds => new Bounds(Center, new Vector3(Radius, Radius, Radius) * 2f);
    }
}