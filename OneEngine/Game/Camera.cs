using System.Collections.Generic;

namespace OneEngine
{
    public sealed class Camera : Component
    {
        static List<Camera> cameras = new List<Camera>();
        public static ReadOnlyArrayList<Camera> Cameras => cameras;

        [BehaviourEvent]
        void Start()
        {
            cameras.Add(this);
        }
        [BehaviourEvent]
        void OnDestroy()
        {
            cameras.Remove(this);
        }

        public Color32 ClearColor { get; set; } = Color32.blue;
        public Vector2 OrthographicSize
        {
            get => Transform.LocalScale;
            set => Transform.LocalScale = value;
        }

        public Matrix3x3 Projection
        {
            get
            {
                Vector2 screenSize = Engine.Instance.Runtime.SystemInfo.ScreenSize;
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
    }
}