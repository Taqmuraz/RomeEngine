namespace RomeEngine
{
    public sealed class Camera : Component
    {
        public static Camera ActiveCamera { get; private set; }

        public Color32 ClearColor { get; set; } = Color32.white * 0.25f;
        public float FieldOfView { get; set; } = 60f;
        public float NearPlane { get; set; } = 0.01f;
        public float FarPlane { get; set; } = 1000f;

        public Matrix4x4 Projection => Matrix4x4.CreateViewport(Screen.Size.x, Screen.Size.y) * Matrix4x4.CreateFrustumMatrix(FieldOfView, Screen.AspectRatio, NearPlane, FarPlane);
        public Matrix4x4 View => Transform.LocalToWorld;

        [BehaviourEvent]
        void Start()
        {
            if (ActiveCamera == null) ActiveCamera = this;
            else throw new System.InvalidOperationException("Active camera already exists, can't create more");
        }
        [BehaviourEvent]
        void OnDestroy()
        {
            ActiveCamera = null;
        }
    }
}