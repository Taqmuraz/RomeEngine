using System;
using System.Linq;

namespace RomeEngine.IO
{
    public sealed class ColladaSkeletonBuilder : IColladaBuilder
    {
        public void BuildGameObject(GameObject gameObject, ColladaEntity rootEntity, ColladaParsingInfo info)
        {
            var visualScenes = rootEntity["library_visual_scenes"]["visual_scene"]["node"];

            foreach (var scene in visualScenes)
            {
                var sceneRoot = new Transform("SceneRoot");
                sceneRoot.Parent = gameObject.Transform;
                var hierarchy = gameObject.AddComponent<ColladaSkeletonHierarchy>();
                ApplyTransform(sceneRoot, scene, hierarchy);
            }
        }
        void ApplyTransform(Transform parent, ColladaEntity node, ColladaSkeletonHierarchy hierarchy)
        {
            string name = node.Properties["name"].Value;
            var matrixNode = node["matrix"];
            Matrix4x4 matrix = matrixNode.IsEmpty ? Matrix4x4.Identity : Matrix4x4.FromFloatsArray(node["matrix"].Single().Value.SeparateString().Select(s => s.ToFloat()).ToArray()).GetTransponed();
            var transform = new Transform(node.Properties["name"].Value);
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
