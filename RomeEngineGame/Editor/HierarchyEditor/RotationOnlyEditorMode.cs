using RomeEngine;
using RomeEngine.UI;

namespace RomeEngineGame
{
    public sealed class RotationOnlyEditorMode : IHierarchyEditorMode
    {
        ITransformHandle rotationHandle = new TransformSingleRotationHandle();
        ITransformHandle positionHandle = new TransformPositionHandle();

        public void DrawHandles(Transform2D transform, Transform2D inspectedTransform, Camera2D camera, Canvas sceneCanvas)
        {
            rotationHandle.Draw(transform, sceneCanvas, camera, IsAccurate);
        }

        public bool IsAccurate { get; set; }
        public string Name => "Rotation";
    }
}