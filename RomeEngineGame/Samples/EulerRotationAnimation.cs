using RomeEngine;

namespace RomeEngineGame
{
    public sealed class EulerRotationAnimation : Component
    {
        [SerializeField] Vector3 euler;

        [BehaviourEvent]
        void Update()
        {
            Transform.LocalRotation += euler * Time.DeltaTime;
        }
    }
}
