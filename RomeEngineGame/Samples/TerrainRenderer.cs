using RomeEngine;

namespace RomeEngineGame
{
    public sealed class TerrainRenderer : Renderer
    {
        [SerializeField] IMesh terrainMesh;
        IMeshIdentifier meshIdentifier;
        [SerializeField] Material terrainMaterial;

        static Vector2 Tiling { get; } = new Vector2(30f, 30f);

        [BehaviourEvent]
        void Start()
        {
            terrainMesh = terrainMesh ?? new StaticMesh
                (
                    new Vertex[]
                    {
                        new Vertex(new Vector3(-0.5f, 0f, -0.5f), Vector3.up, new Vector2(0f, 0f)),
                        new Vertex(new Vector3(0.5f, 0f, -0.5f), Vector3.up, new Vector2(Tiling.x, 0f)),
                        new Vertex(new Vector3(0.5f, 0f, 0.5f), Vector3.up, Tiling),
                        new Vertex(new Vector3(-0.5f, 0f, 0.5f), Vector3.up, new Vector2(0f, Tiling.y)),
                    },
                    new int[] { 0, 2, 1, 0, 3, 2 }
                );
            Transform.LocalScale = new Vector3(100f, 100f, 100f);
            terrainMaterial = terrainMaterial ?? new SingleTextureMaterial("Terrain material") { TextureFileName = "./Resources/Textures/Grass.jpg" };
        }

        protected override void VisitContext(IGraphicsContext context)
        {
            if (terrainMesh != null) meshIdentifier = context.LoadMesh(terrainMesh);
            terrainMaterial.VisitContext(context);
        }

        protected override void Draw(IGraphics graphics)
        {
            if (meshIdentifier != null)
            {
                graphics.SetCulling(CullingMode.Back);
                terrainMaterial.PrepareDraw(graphics);
                graphics.DrawMesh(meshIdentifier);
            }
        }
    }
}
