using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{

    public sealed class ColladaGeometryParsingContext : ColladaParsingContext<ColladaGeometryParsingContext, ColladaRawMesh>, IColladaParsingStage
    {
        public static string MaterialPostfix { get; } = "_material";

        public ColladaGeometryParsingContext(ColladaSemanticModel semanticModel) : base(semanticModel)
        {
        }

        public ColladaControllersParsingContext CreateControllersContext()
        {
            return new ColladaControllersParsingContext(Elements, SemanticModel);
        }

        protected override IEnumerable<IColladaNodeHandler<ColladaGeometryParsingContext>> CreateHandlers()
        {
            void HandlePolygon(ColladaGeometryParsingContext context, IColladaNode node, PolygonFormat format)
            {
                string materialName = node.GetAttribute("material") + MaterialPostfix;
                context.CurrentMesh.TrianglesData.PushElement(new ColladaPolygonsData(materialName, format));
            }
            return new IColladaNodeHandler<ColladaGeometryParsingContext>[]
            {

                new ColladaDelegateHandler<ColladaGeometryParsingContext>("geometry", (context, node) => context.PushElement(new ColladaRawMesh(node.GetAttribute("id"))), (context, node) => context.PopElement()),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("source", (context, node) => context.CurrentMesh.PushElement(new ColladaVertexBuffer(node.GetAttribute("id"))), (context, node) => context.CurrentMesh.PopElement()),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("float_array", (context, node) => context.CurrentMesh.WriteBuffer(node.GetValue()), null),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("accessor", (context, node) => context.CurrentMesh.WriteAttribute(new ColladaVertexAttribute(node.GetAttribute("source"))), null),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("param", (context, node) => context.CurrentMesh.WriteAttributeProperty(node.GetAttribute("name"), node.GetAttribute("type")), null),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("triangles", (c, n) => HandlePolygon(c, n, PolygonFormat.Triangles), (context, node) => context.CurrentMesh.TrianglesData.PopElement()),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("polylist", (c, n) => HandlePolygon(c, n, PolygonFormat.Polygons), (context, node) => context.CurrentMesh.TrianglesData.PopElement()),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("p", (context, node) => context.CurrentMesh.TrianglesData.CurrentElement.Indices = node.GetValue(), null),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("input", (context, node) => context.SemanticModel.AddSemantic(new ColladaSemantic(node.GetAttribute("semantic"), node.GetAttribute("source"))), null),
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
                    if (mesh.BuildMesh(i, SemanticModel, out var addFunc))
                    {
                        addFunc(gameObject, new SingleTextureMaterial(mesh.TrianglesData.Elements[i].MaterialName));
                    }
                }
            }
        }

        protected override ColladaGeometryParsingContext GetContext() => this;
    }
}
