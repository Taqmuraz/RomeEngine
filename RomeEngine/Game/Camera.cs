using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class Camera : Component
    {
        static List<Camera> cameras = new List<Camera>();
        public static ReadOnlyArrayList<Camera> Cameras => cameras;

        [BehaviourEvent]
        void Start()
        {
            cameras.Add(this);
            OrthographicMultiplier = 1f;
        }
        [BehaviourEvent]
        void OnDestroy()
        {
            cameras.Remove(this);
        }

        public Color32 ClearColor { get; set; } = Color32.white * 0.25f;

        public Vector2 OrthographicSize { get; private set; }

        public float OrthographicMultiplier
        {
            get => orthographicMultiplier;
            set
            {
                orthographicMultiplier = value;
                OrthographicSize = new Vector2(value, value) * Screen.AspectRatio * 2f;
                Transform.LocalScale = new Vector2(value, value);
            }
        }
        float orthographicMultiplier;

        public Matrix3x3 Projection
        {
            get
            {
                Vector2 screenSize = Screen.Size;
                return Matrix3x3.Viewport(screenSize.x, screenSize.y);
            }
        }

        Matrix3x3 ViewMatrix => Transform.LocalToWorld;

        public Vector2 WorldToScreen(Vector2 world)
        {
            return WorldToScreenMatrix.MultiplyPoint(world);
        }
        public Vector2 ScreenToWorld(Vector2 world)
        {
            return ScreenToWorldMatrix.MultiplyPoint(world);
        }

        public Matrix3x3 WorldToScreenMatrix => Projection * ViewMatrix.GetInversed();
        public Matrix3x3 ScreenToWorldMatrix => ViewMatrix * Projection.GetInversed();

        public Rect Volume => Rect.FromCenterAndSize(Transform.LocalPosition, OrthographicSize);
    }
}