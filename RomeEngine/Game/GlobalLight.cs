namespace RomeEngine
{
    public sealed class GlobalLight : Component
    {
        public static Vector3 LightDirection { get; private set; }

        [BehaviourEvent]
        void OnPreRender()
        {
            LightDirection = Transform.Forward;
        }
    }
}