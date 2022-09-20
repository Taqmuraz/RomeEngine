using RomeEngine;
using RomeEngine.UI;

namespace RomeEngineGame
{
    public interface IHierarchyEditorMode
    {
        string Name { get; }
        void DrawHandles(Transform transform, Transform inspectedTransform, Camera camera, Canvas sceneCanvas);
        bool IsAccurate { get; set; }
    }
}