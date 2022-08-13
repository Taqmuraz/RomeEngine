﻿using OneEngine;
using OneEngine.UI;

namespace OneEngineGame
{
    public sealed class RotationOnlyEditorMode : IHierarchyEditorMode
    {
        ITransformHandle rotationHandle = new TransformSingleRotationHandle();

        public void DrawHandles(Transform transform, Transform inspectedTransform, Camera camera, Canvas sceneCanvas)
        {
            if (transform.GameObject.Layer == Layer.Bone.Index) rotationHandle.Draw(transform, sceneCanvas, camera, IsAccurate);
        }

        public bool IsAccurate { get; set; }
        public string Name => "Rotation";
    }
}