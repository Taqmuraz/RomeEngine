namespace RomeEngine
{
    public sealed class SkinnedMeshRenderer : MeshRenderer<SkinnedMesh>
    {
        [SerializeField] public SkinnedMesh SkinnedMesh { get; set; }
        protected override SkinnedMesh Mesh => SkinnedMesh;
    }
}
