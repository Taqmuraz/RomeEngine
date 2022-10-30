using RomeEngine;

namespace RomeEngineGame
{
    public sealed class EulerRotationAnimation : Component
    {
        [SerializeField] Vector3 euler;

        public EulerRotationAnimation()
        {
        }

        [BehaviourEvent]
        void Update()
        {
            Transform.Rotation += euler * Time.DeltaTime;
        }
    }
}
