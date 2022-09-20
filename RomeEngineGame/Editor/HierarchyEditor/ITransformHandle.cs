using RomeEngine;
using RomeEngine.UI;

namespace RomeEngineGame
{
    public interface ITransformHandle
    {
        bool Draw(Transform transform, Canvas canvas, Camera camera, bool accurateMode);
    }
}