namespace RomeEngine
{
    public sealed class GlobalLight : Component
    {
        public static Vector3 LightDirection { get; private set; }
        public static Color32 LightColor { get; set; }
        public static float AmbienceIntencivity { get; set; }
        public static float LightIntencivity { get; set; }

        [SerializeField] float ambienceIntencivity = 0.3f;
        [SerializeField] float lightIntencivity = 0.3f;
        [SerializeField] Color32 lightColor = new Color32(1f, 0.8f, 0.6f, 1f);

        [BehaviourEvent]
        void OnPreRender()
        {
            LightDirection = Transform.Forward;
            LightColor = lightColor;
            AmbienceIntencivity = ambienceIntencivity;
            LightIntencivity = lightIntencivity;
        }

        public void Setup(float ambienceIntencivity, float lightIntencivity, Color32 color)
        {
            this.ambienceIntencivity = ambienceIntencivity;
            this.lightIntencivity = lightIntencivity;
            lightColor = color;
        }
    }
}