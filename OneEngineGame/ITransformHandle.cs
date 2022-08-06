using OneEngine;
using OneEngine.UI;

namespace OneEngineGame
{
    public interface ITransformHandle
    {
        bool Draw(Transform transform, Canvas canvas, Camera camera, bool accurateMode);
    }
}