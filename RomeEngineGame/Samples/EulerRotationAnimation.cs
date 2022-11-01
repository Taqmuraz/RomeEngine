using RomeEngine;

namespace RomeEngineEditor
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
