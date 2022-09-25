using System.Collections.Generic;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaVisualSceneParsingContext : ColladaParsingContext<ColladaVisualSceneParsingContext, ColladaTransformInfo>
    {
        ReadOnlyArrayList<ColladaController> previousStageControllers;

        public ColladaVisualSceneParsingContext(ReadOnlyArrayList<ColladaController> previousStageControllers)
        {
            this.previousStageControllers = previousStageControllers;
        }

        protected override IEnumerable<IColladaNodeHandler<ColladaVisualSceneParsingContext>> CreateHandlers()
        {
            yield return new ColladaDelegateHandler<ColladaVisualSceneParsingContext>("visual_scene", (context, node) => context.PushElement(new ColladaTransformInfo("visual_scene_root", context.StackDepth)), (context, node) => context.PopElement());
            yield return new ColladaDelegateHandler<ColladaVisualSceneParsingContext>("node", (context, node) =>
            {
                var transform = new ColladaTransformInfo(node.GetAttribute("name"), context.StackDepth);
                context.CurrentElement.Children.Add(transform);
                context.PushElement(transform);
            }, (context, node) => PopElement());
            yield return new ColladaDelegateHandler<ColladaVisualSceneParsingContext>("matrix", (context, node) => context.CurrentElement.Matrix = node.GetValue(), null);
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
            var joints = previousStageControllers.SelectMany(c => c.Elements).Select(s => (matrices:s.ReadJoints(out string[] names), names: names));
            var search = joints.Select(j => (index:j.names.IndexOf(info.Name),info:j));

            if (search.Any(s => s.index >= 0))
            {
                var result = search.First(s => s.index >= 0);
                var matrix = result.info.matrices[result.index];
                transform.ApplyMatrix(matrix);
            }
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
