namespace RomeEngine
{
    public sealed class GenericMeshRenderer : MeshRenderer<IMesh>
    {
        [SerializeField] public IMesh GenericMesh { get; set; }
        protected override IMesh Mesh => GenericMesh;

        protected override void DrawCall(IGraphics graphics, IMeshIdentifier meshIdentifier)
        {
            base.DrawCall(graphics, meshIdentifier);
        }
    }
}
