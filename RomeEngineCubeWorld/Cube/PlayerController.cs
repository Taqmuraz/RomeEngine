using RomeEngine;

namespace RomeEngineCubeWorld
{
    public sealed class PlayerController : Component
    {
        Vector3 euler;
        Camera camera;
        ICubeWorld cubeWorld;
        float moveSpeed = 3f;

        [BehaviourEvent]
        void Start()
        {
            camera = Camera.ActiveCamera;
            cubeWorld = CubeWorld.Instance;
        }
        [BehaviourEvent]
        void Update()
        {
            Vector2 mouse = Input.MouseDelta * Time.DeltaTime * 15f;
            euler += new Vector3(mouse.y, mouse.x, 0f);
            euler.x = Mathf.Clamp(euler.x, -80f, 80f);
            camera.Transform.Rotation = euler;
            camera.Transform.Position = Transform.Position + new Vector3(0f, 1.5f, 0f);

            Vector2 wasd = Input.GetWASD();
            Transform.Rotation = new Vector3(0f, euler.y, 0f);
            Transform.Position += Transform.LocalToWorld.MultiplyDirection(new Vector3(wasd.x, 0f, wasd.y)) * Time.DeltaTime * moveSpeed;

            Input.CursorState = CursorState.HiddenAndLocked;

            if (Input.GetKeyDown(KeyCode.MouseL))
            {
                cubeWorld.RaycastCubeAsync(new Ray(camera.Transform.Position, camera.Transform.Forward), cube =>
                {
                    cubeWorld.ModifyCube(RemoveCubeModifier.Instance, cube.Position);
                });
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Engine.Quit();
            }
        }
    }
}
