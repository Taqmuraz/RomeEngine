using RomeEngine;
using RomeEngine.UI;

namespace RomeEngineGame
{
    public class FullModelEditorMode : IHierarchyEditorMode
    {
        public bool IsAccurate { get; set; }

        ITransformHandle[] transformHandles =
            {
                new TransformRotationHandle(),
                new TransformPositionHandle(),
                new TransformScaleHandle() { Axis = 1 },
                new TransformScaleHandle() { Axis = 2 },
                new TransformScaleHandle() { Axis = 3 },
            };

        public void DrawHandles(Transform2D transform, Transform2D inspectedTransform, Camera2D camera, Canvas sceneCanvas)
        {
            var worldToScreen = camera.WorldToScreenMatrix;
            var l2w = transform.LocalToWorld;
            
            sceneCanvas.DrawLine(worldToScreen.MultiplyPoint((Vector2)l2w.Column_2), worldToScreen.MultiplyPoint(l2w.MultiplyPoint(Vector2.right)), transform == inspectedTransform ? Color32.white : Color32.blue, 1);
            if (transform == inspectedTransform) foreach (var handle in transformHandles) handle.Draw(transform, sceneCanvas, camera, IsAccurate);
        }

        public string Name => "Full";
    }
}