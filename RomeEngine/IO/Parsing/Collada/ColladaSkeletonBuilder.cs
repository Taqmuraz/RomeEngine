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
                var sceneRoot = new GameObject("SceneRoot").Transform;
                sceneRoot.Parent = gameObject.Transform;
                ApplyTransform(sceneRoot, scene);
            }

            foreach (var skinnedMesh in gameObject.GetComponentsOfType<SkinnedMeshRenderer>())
            {
                skinnedMesh.InitializeBindings();
            }
        }
        void ApplyTransform(Transform transform, ColladaEntity node)
        {
            transform.GameObject.Name = node.Properties["name"].Value;
            var matrixNode = node["matrix"];
            Matrix4x4 matrix = matrixNode.IsEmpty ? Matrix4x4.Identity : Matrix4x4.FromFloatsArray(node["matrix"].Single().Value.SeparateString().Select(s => s.ToFloat()).ToArray()).GetTransponed();
            transform.ApplyMatrix(matrix);
            foreach (var child in node["node"])
            {
                var newChild = new GameObject("NewChild").Transform;
                newChild.Parent = transform;
                ApplyTransform(newChild, child);
            }
        }
    }
}
