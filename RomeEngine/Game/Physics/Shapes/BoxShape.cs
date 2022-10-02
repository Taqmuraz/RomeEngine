namespace RomeEngine
{
    public sealed class BoxShape : IColliderShape
    {
        public Vector3 Center { get; set; }
        public Vector3 Size { get; set; }
        public Vector3 Rotation { get; set; }

        public ColliderShapeType ShapeType => ColliderShapeType.Box;
        public Bounds Bounds => new Bounds(Center, Size).Rotate(Rotation);
    }
}