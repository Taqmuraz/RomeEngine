namespace RomeEngine
{
    public sealed class StaticMeshRenderer : Renderer
    {
        [SerializeField] public StaticMesh StaticMesh { get; set; }
        IMeshIdentifier meshIdentifier;

        protected override void VisitContext(IGraphicsContext context)
        {
            meshIdentifier = StaticMesh == null ? null : context.LoadMesh(StaticMesh);
        }
        protected override void Draw(IGraphics graphics)
        {
            if (meshIdentifier != null) graphics.DrawMesh(meshIdentifier);
        }
    }
}
