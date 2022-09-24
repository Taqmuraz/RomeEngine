namespace RomeEngine
{
    public abstract class MeshRenderer<TMesh> : Renderer where TMesh : IMesh
    {
        [SerializeField] public bool CullBackFaces { get; set; } = true;
        protected abstract TMesh Mesh { get; }
        IMeshIdentifier meshIdentifier;

        protected override void VisitContext(IGraphicsContext context)
        {
            var mesh = Mesh;
            meshIdentifier = mesh == null ? null : context.LoadMesh(mesh);
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
