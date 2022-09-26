namespace RomeEngine
{
    public sealed class StaticBufferMeshRenderer : MeshRenderer<StaticBufferMesh>
    {
        [SerializeField] public StaticBufferMesh StaticBufferMesh { get; set; }
        protected override StaticBufferMesh Mesh => StaticBufferMesh;
    }
}
