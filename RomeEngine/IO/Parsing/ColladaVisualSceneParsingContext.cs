using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaVisualSceneParsingContext : ColladaParsingContext<ColladaVisualSceneParsingContext, ColladaTransformInfo>
    {
        protected override IEnumerable<IColladaNodeHandler<ColladaVisualSceneParsingContext>> CreateHandlers()
        {
            yield return new ColladaDelegateHandler<ColladaVisualSceneParsingContext>("visual_scene", (context, node) => context.PushElement(new ColladaTransformInfo("visual_scene_root", context.StackDepth)), (context, node) => context.PopElement());
            yield return new ColladaDelegateHandler<ColladaVisualSceneParsingContext>("node", (context, node) =>
            {
                var transform = new ColladaTransformInfo(node.GetAttribute("name"), context.StackDepth);
                context.CurrentElement.Children.Add(transform);
                context.PushElement(transform);
            }, (context, node) => PopElement().UpdateChildren());
            yield return new ColladaDelegateHandler<ColladaVisualSceneParsingContext>("matrix", (context, node) => context.CurrentElement.Matrix = node.GetValue(), null);
            yield return new ColladaDelegateHandler<ColladaVisualSceneParsingContext>("tip_x", (context, node) => context.CurrentElement.TipX = node.GetValue().ToFloat(), null);
            yield return new ColladaDelegateHandler<ColladaVisualSceneParsingContext>("tip_y", (context, node) => context.CurrentElement.TipY = node.GetValue().ToFloat(), null);
            yield return new ColladaDelegateHandler<ColladaVisualSceneParsingContext>("tip_z", (context, node) => context.CurrentElement.TipZ = node.GetValue().ToFloat(), null);
        }

        protected override ColladaVisualSceneParsingContext GetContext() => this;

        public override void UpdateGameObject(GameObject gameObject, IColladaParsingInfo parsingInfo)
        {
            var root = new ColladaTransformInfo("SceneRootNode", -1);
            root.Children.AddRange(Elements.Where(e => e.Depth == 0));
            ApplyTransform(root, gameObject.Transform);
        }
        void ApplyTransform(ColladaTransformInfo info, Transform transform)
        {
            transform.ApplyMatrix(info.ReadMatrix());
            transform.LocalPosition += info.StartOffset;

            foreach (var child in info.Children)
            {
                var childTransform = new GameObject(child.Name).Transform;
                childTransform.Parent = transform;
                ApplyTransform(child, childTransform);
            }
        }

        public override string RootNodeName => "library_visual_scenes";
    }
}
