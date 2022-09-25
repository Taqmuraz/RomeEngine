namespace RomeEngine
{
    public abstract class MeshRenderer<TMesh> : Renderer where TMesh : IMesh
    {
        [SerializeField] public Material Material { get; set; }
        [SerializeField] public bool CullBackFaces { get; set; } = true;
        protected abstract TMesh Mesh { get; }
        IMeshIdentifier meshIdentifier;

        protected override void VisitContext(IGraphicsContext context)
        {
            var mesh = Mesh;
            meshIdentifier = mesh == null ? null : context.LoadMesh(mesh);
            if (Material != null) Material.VisitContext(context);
        }
        protected override void Draw(IGraphics graphics)
        {
            if (meshIdentifier != null)
            {
                if (Material != null) Material.PrepareDraw(graphics);
                else graphics.SetTexture(null, TextureType.Albedo);
                graphics.SetCulling(CullBackFaces ? CullingMode.Back : CullingMode.None);
                
            }
        }
        protected virtual void DrawCall(IGraphics graphics, IMeshIdentifier meshIdentifier)
        {
            graphics.DrawMesh(meshIdentifier);
        }
    }
}
