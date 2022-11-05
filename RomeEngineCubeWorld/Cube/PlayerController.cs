using RomeEngine;

namespace RomeEngineCubeWorld
{
    public sealed class PlayerController : Component
    {
        Vector3 euler;
        Camera camera;
        float moveSpeed = 5f;

        [BehaviourEvent]
        void Start()
        {
            camera = Camera.ActiveCamera;
        }
        [BehaviourEvent]
        void Update()
        {
            var cubeWorld = CubeWorld.Instance;
            if (cubeWorld == null) return;

            if (Input.GetKeyDown(KeyCode.Space)) euler = new Vector3();

            float moveSpeed = this.moveSpeed;

            if (Input.GetKey(KeyCode.ShiftKey)) moveSpeed *= 10f;

            Vector2 mouse = Input.MouseDelta * Time.DeltaTime * 15f;
            euler += new Vector3(mouse.y, mouse.x, 0f);
            euler.x = Mathf.Clamp(euler.x, -80f, 80f);
            camera.Transform.Rotation = euler;
            camera.Transform.Position = Transform.Position + new Vector3(0f, 1.5f, 0f);

            Vector3 wasdqe = Input.GetWASDQE();
            Transform.Rotation = new Vector3(0f, euler.y, 0f);
            Transform.Position += Transform.LocalToWorld.MultiplyDirection(wasdqe) * Time.DeltaTime * moveSpeed;

            Input.CursorState = CursorState.HiddenAndLocked;

            var cameraRay = new Ray(camera.Transform.Position, camera.Transform.Forward);

            if (Input.GetKeyDown(KeyCode.MouseL))
            {
                cubeWorld.RaycastCubeAsync(cameraRay, cube =>
                {
                    cubeWorld.ModifyCube(RemoveCubeModifier.Instance, cube.Position);
                });
            }
            if (Input.GetKeyDown(KeyCode.MouseR))
            {
                cubeWorld.RaycastCubeAsync(cameraRay, cube =>
                {
                    if (cube.Bounds.GetNormalForRay(cameraRay, out Vector3 normal))
                    {
                        cubeWorld.ModifyCube(new CubeIdModifier(4), cube.Position + CubeCoords.CubeDirection(normal));
                    }
                });
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Engine.Quit();
            }
        }
    }
}
