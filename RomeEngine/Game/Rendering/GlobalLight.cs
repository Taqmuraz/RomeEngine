namespace RomeEngine
{
    public sealed class GlobalLight : Component
    {
        public static Vector3 LightDirection { get; private set; }
        public static Color32 LightColor { get; set; } = new Color32(1f, 0.8f, 0.6f, 1f);
        public static float AmbienceIntencivity { get; set; } = 0.3f;
        [BehaviourEvent]
        void OnPreRender()
        {
            LightDirection = Transform.Forward;
        }
    }
}