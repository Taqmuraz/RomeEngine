using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaGeometryParsingContext : ColladaStackContainingObject<ColladaRawMesh>, IColladaParsingStage
    {
        Dictionary<string, IColladaNodeHandler<ColladaGeometryParsingContext>> handlers = new Dictionary<string, IColladaNodeHandler<ColladaGeometryParsingContext>>();

        public ColladaGeometryParsingContext()
        {
            handlers = new IColladaNodeHandler<ColladaGeometryParsingContext>[]
            {
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("geometry", (context, node) => context.PushElement(new ColladaRawMesh(node.GetAttribute("id"))), (context, node) => context.PopElement()),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("source", (context, node) => context.CurrentMesh.PushElement(new ColladaVertexBuffer(node.GetAttribute("id"))), (context, node) => context.CurrentMesh.PopElement()),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("float_array", (context, node) => context.CurrentMesh.WriteBuffer(node.GetValue()), null),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("accessor", (context, node) => context.CurrentMesh.WriteAttribute(new ColladaVertexAttribute(node.GetAttribute("source"))), null),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("param", (context, node) => context.CurrentMesh.WriteAttributeProperty(node.GetAttribute("name"), node.GetAttribute("type")), null),
                new ColladaDelegateHandler<ColladaGeometryParsingContext>("p", (context, node) => context.CurrentMesh.WriteIndices(node.GetValue()), null)
            }
            .ToDictionary(h => h.Name);
        }

        public string RootNodeName => "library_geometries";

        public void ParseStart(IColladaNode node)
        {
            if (handlers.TryGetValue(node.GetName(), out var handler))
            {
                handler.ParseStart(this, node);
            }
        }
        public void ParseEnd(IColladaNode node)
        {
            if (handlers.TryGetValue(node.GetName(), out var handler))
            {
                handler.ParseEnd(this, node);
            }
        }

        public ColladaRawMesh CurrentMesh => CurrentElement;

        public void UpdateGameObject(GameObject gameObject)
        {
            foreach (var mesh in Elements)
            {
                for (int i = 0; i < mesh.SubmeshesCount; i++)
                {
                    var renderer = gameObject.AddComponent<SkinnedMeshRenderer>();
                    renderer.SkinnedMesh = mesh.BuildMesh(i);
                }
            }
        }
    }
}
