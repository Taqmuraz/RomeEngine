using RomeEngine;
using RomeEngine.UI;

namespace RomeEngineGame
{
    public interface ITransformHandle
    {
        bool Draw(Transform2D transform, Canvas canvas, Camera2D camera, bool accurateMode);
    }
}