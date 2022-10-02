namespace RomeEngine
{
    public sealed class SphereCollider : Collider
    {
        [SerializeField] public Vector3 LocalCenter { get; set; }
        [SerializeField] public float LocalRadius { get; set; } = 1f;

        SphereShape shape = new SphereShape();
        protected override IColliderShape Shape => shape;

        protected override void UpdateShape()
        {
            var l2w = Transform.LocalToWorld;
            shape.Center = l2w.MultiplyPoint3x4(LocalCenter);
            shape.Radius = LocalRadius;//l2w.GetDeterminant() * LocalRadius;
        }
    }
}