using RomeEngine;

namespace RomeEngineCubeWorld
{
    public sealed class CubeChunkMeshRenderer : Renderer
    {
        bool hasToUpdate;
        IMesh cubeChunkMesh;
        IMeshIdentifier meshIdentifier;
        public Material Material { get; set; }

        public void UpdateMesh(IMesh mesh)
        {
            hasToUpdate = true;
            cubeChunkMesh = mesh;
        }

        protected override void VisitContext(IGraphicsContext context)
        {
            if (Material != null) Material.VisitContext(context);
            if (hasToUpdate)
            {
                if (meshIdentifier != null) context.UnloadMesh(meshIdentifier);
                meshIdentifier = context.LoadMesh(cubeChunkMesh);

                hasToUpdate = false;
            }
        }

        protected override void Draw(IGraphics graphics)
        {
            if (meshIdentifier != null)
            {
                if (Material != null) Material.PrepareDraw(graphics);
                else graphics.SetTexture(null, TextureType.Albedo);
                graphics.SetCulling(CullingMode.Back);
                graphics.DrawMesh(meshIdentifier);
            }
        }
    }
}
