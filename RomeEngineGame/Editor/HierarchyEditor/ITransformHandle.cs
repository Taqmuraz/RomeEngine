using RomeEngine;
using RomeEngine.UI;

namespace RomeEngineGame
{
    public interface ITransformHandle
    {
        void DrawHandle(HierarchyTransform transform, Canvas canvas);
    }
}