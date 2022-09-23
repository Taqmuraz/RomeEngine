namespace RomeEngine
{
    public sealed class StaticMeshRenderer : Renderer
    {
        [SerializeField] public bool CullBackFaces { get; set; } = true;
        [SerializeField] public StaticMesh StaticMesh { get; set; }
        IMeshIdentifier meshIdentifier;

        protected override void VisitContext(IGraphicsContext context)
        {
            meshIdentifier = StaticMesh == null ? null : context.LoadMesh(StaticMesh);
        }
        protected override void Draw(IGraphics graphics)
        {
            if (meshIdentifier != null)
            {
                graphics.SetTexture(null, TextureType.Albedo);
                graphics.SetCulling(CullBackFaces ? CullingMode.Back : CullingMode.None);
                graphics.DrawMesh(meshIdentifier);
            }
        }
    }
}
