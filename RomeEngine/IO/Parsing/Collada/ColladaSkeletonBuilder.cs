using System;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaSkeletonBuilder : IColladaBuilder
    {
        public void BuildGameObject(GameObject gameObject, ColladaEntity rootEntity, ColladaParsingInfo info)
        {
            var visualScenes = rootEntity["library_visual_scenes"]["visual_scene"]["node"];

            var root = new HierarchyTransform("SceneRoot");
            root.Parent = gameObject.Transform;
            var hierarchy = gameObject.AddComponent<ColladaSkeletonHierarchy>();
            hierarchy.Root = root;

            foreach (var scene in visualScenes)
            {
                ApplyTransform(root, scene, hierarchy);
            }
        }
        void ApplyTransform(HierarchyTransform parent, ColladaEntity node, ColladaSkeletonHierarchy hierarchy)
        {
            string name = node.Properties["name"].Value;
            var matrixNode = node["matrix"];
            Matrix4x4 matrix = matrixNode.IsEmpty ? Matrix4x4.Identity : Matrix4x4.FromFloatsArray(node["matrix"].Single().Value.SeparateString().Select(s => s.ToFloat()).ToArray()).GetTransponed();
            var transform = new HierarchyTransform(node.Properties["name"].Value);
            transform.Parent = parent;
            transform.ApplyMatrix(matrix);

            hierarchy.AddBone(new SkeletonBone(name, transform));

            foreach (var child in node["node"])
            {
                ApplyTransform(transform, child, hierarchy);
            }
        }
    }
}
