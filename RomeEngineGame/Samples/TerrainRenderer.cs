using RomeEngine;

namespace RomeEngineGame
{
    public sealed class TerrainRenderer : Renderer
    {
        IMesh terrainMesh;
        IMeshIdentifier meshIdentifier;
        Texture terrainTexture;

        static Vector2 Tiling { get; } = new Vector2(30f, 30f);

        [BehaviourEvent]
        void Start()
        {
            terrainMesh = new StaticMesh
                (
                    new Vertex[] 
                    {
                        new Vertex(new Vector3(-0.5f, 0f, -0.5f), Vector3.up, new Vector2(0f, 0f)),
                        new Vertex(new Vector3(0.5f, 0f, -0.5f), Vector3.up, new Vector2(Tiling.x, 0f)),
                        new Vertex(new Vector3(0.5f, 0f, 0.5f), Vector3.up, Tiling),
                        new Vertex(new Vector3(-0.5f, 0f, 0.5f), Vector3.up, new Vector2(0f, Tiling.y)),
                    },
                    new int[] { 0, 2, 1, 0, 3, 2}
                );
            Transform.LocalScale = new Vector3(100f, 100f, 100f);
        }

        protected override void VisitContext(IGraphicsContext context)
        {
            if (meshIdentifier == null && terrainMesh != null) meshIdentifier = context.LoadMesh(terrainMesh);
            terrainTexture = context.LoadTexture("./Resources/Textures/Grass.jpg");
        }

        protected override void Draw(IGraphics graphics)
        {
            if (meshIdentifier != null)
            {
                graphics.SetCulling(CullingMode.Back);
                graphics.SetTexture(terrainTexture, TextureType.Albedo);
                graphics.DrawMesh(meshIdentifier);
            }
        }
    }
}
