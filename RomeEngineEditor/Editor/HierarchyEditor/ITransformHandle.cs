using RomeEngine;
using RomeEngine.UI;

namespace RomeEngineEditor
{
    public interface ITransformHandle
    {
        void DrawHandle(HierarchyTransform transform, Canvas canvas);
    }
}