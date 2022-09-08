using RomeEngine;

namespace OneEngineGame
{
    public sealed class PlayerController : HumanController
    {
        float orthographicSize = 5f;
        Camera camera;

        [BehaviourEvent]
        void Start()
        {
            camera = Camera.Cameras[0];
            camera.OrthographicMultiplier = orthographicSize;
        }
    }
}
