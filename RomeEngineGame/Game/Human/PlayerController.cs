using RomeEngine;

namespace OneEngineGame
{
    public sealed class PlayerController : HumanController, IControlAgent
    {
        float orthographicSize = 5f;
        Camera camera;

        [BehaviourEvent]
        void Start()
        {
            camera = Camera.Cameras[0];
            camera.OrthographicMultiplier = orthographicSize;
        }

        public override IControlAgent GetControlAgent() => this;

        public Vector2 InputMovement => Input.GetWASD();
    }
}
