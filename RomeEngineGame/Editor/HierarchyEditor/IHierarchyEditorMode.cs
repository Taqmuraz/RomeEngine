using RomeEngine;
using RomeEngine.UI;

namespace RomeEngineGame
{
    public interface IHierarchyEditorMode
    {
        string Name { get; }
        void DrawHandles(Transform2D transform, Transform2D inspectedTransform, Camera2D camera, Canvas sceneCanvas);
        bool IsAccurate { get; set; }
    }
}