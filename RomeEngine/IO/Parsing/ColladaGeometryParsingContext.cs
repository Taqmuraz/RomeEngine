using System.Collections.Generic;

namespace RomeEngine.IO
{
    public sealed class ColladaGeometryParsingContext : ColladaParsingContext<ColladaGeometryParsingContext, ColladaRawMesh>, IColladaParsingStage
    {
        public ColladaControllersParsingContext CreateControllersContext()
        {
            return new ColladaControllersParsingContext(Elements);
        }

        protected override IEnumerable<IColladaNodeHandler<ColladaGeometryParsingContext>> CreateHandlers()
        {
            return new IColladaNodeHandler<ColladaGeometryParsingContext>[]
            {
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("geometry", (context, node) => context.PushElement(new ColladaRawMesh(node.GetAttribute("id"))), (context, node) => context.PopElement()),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("source", (context, node) => context.CurrentMesh.PushElement(new ColladaVertexBuffer(node.GetAttribute("id"))), (context, node) => context.CurrentMesh.PopElement()),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("float_array", (context, node) => context.CurrentMesh.WriteBuffer(node.GetValue()), null),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("accessor", (context, node) => context.CurrentMesh.WriteAttribute(new ColladaVertexAttribute(node.GetAttribute("source"))), null),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("param", (context, node) => context.CurrentMesh.WriteAttributeProperty(node.GetAttribute("name"), node.GetAttribute("type")), null),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("triangles", (context, node) => context.CurrentMesh.TrianglesData.PushElement(new TrianglesData(node.GetAttribute("material"))), (context, node) => context.CurrentMesh.TrianglesData.PopElement()),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("p", (context, node) => context.CurrentMesh.TrianglesData.CurrentElement.Indices = node.GetValue(), null),
            };
        }

        public override string RootNodeName => "library_geometries";

        public ColladaRawMesh CurrentMesh => CurrentElement;

        public override void UpdateGameObject(GameObject gameObject, IColladaParsingInfo parsingInfo)
        {
            foreach (var mesh in Elements)
            {
                for (int i = 0; i < mesh.SubmeshesCount; i++)
                {
                    var renderer = gameObject.AddComponent<SkinnedMeshRenderer>();
                    renderer.SkinnedMesh = mesh.BuildMesh(i);
                    renderer.Material = new SingleTextureMaterial(mesh.TrianglesData.Elements[i].MaterialName);
                }
            }
        }

        protected override ColladaGeometryParsingContext GetContext() => this;
    }
}
