using System.Collections.Generic;

namespace RomeEngine
{
    public abstract class Camera<TCamera> : Component where TCamera : Camera<TCamera>
    {
        static List<TCamera> cameras = new List<TCamera>();
        public static TCamera ActiveCamera { get; private set; }

        [BehaviourEvent]
        void Start()
        {
            if (ActiveCamera == null) ActiveCamera = (TCamera)this;
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