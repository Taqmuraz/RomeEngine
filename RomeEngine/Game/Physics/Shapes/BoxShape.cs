namespace RomeEngine
{
    public sealed class BoxShape : IPhysicalShape
    {
        public Vector3 Center { get; set; }
        public Vector3 Size { get; set; }
        public Vector3 Rotation { get; set; }

        public PhysicalShapeType ShapeType => PhysicalShapeType.Box;
        public Bounds Bounds => new Bounds(Center, Size).Rotate(Rotation);
    }
}