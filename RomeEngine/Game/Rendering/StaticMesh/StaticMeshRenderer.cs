namespace RomeEngine
{
    public sealed class StaticMeshRenderer : MeshRenderer<StaticMesh>
    {
        [SerializeField] public StaticMesh StaticMesh { get; set; }
        protected override StaticMesh Mesh => StaticMesh;
    }
}
