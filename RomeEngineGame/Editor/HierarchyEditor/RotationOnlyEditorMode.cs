using RomeEngine;
using RomeEngine.UI;

namespace RomeEngineGame
{
    public sealed class RotationOnlyEditorMode : IHierarchyEditorMode
    {
        ITransformHandle rotationHandle = new TransformSingleRotationHandle();
        ITransformHandle positionHandle = new TransformPositionHandle();

        public void DrawHandles(Transform transform, Transform inspectedTransform, Camera camera, Canvas sceneCanvas)
        {
            rotationHandle.Draw(transform, sceneCanvas, camera, IsAccurate);
        }

        public bool IsAccurate { get; set; }
        public string Name => "Rotation";
    }
}