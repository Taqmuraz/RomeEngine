namespace RomeEngine
{
    public sealed class Camera : Component
    {
        public static Camera ActiveCamera { get; private set; }

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

        public Color32 ClearColor { get; set; } = Color32.white * 0.25f;
    }
}