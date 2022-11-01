using RomeEngine;

namespace RomeEngineEditor
{
    public abstract class HumanMovementState : HumanState
    {
        protected abstract string GetMovementAnimationName();
        protected abstract string GetIdleAnimationName();
        protected abstract float MovementSpeed { get; }

        [BehaviourEvent]
        void Update()
        {
            var inputMovement = HumanController.GetControlAgent().InputMovement.normalized;
            if (inputMovement != new Vector2())
            {
                HumanController.HumanAnimator.PlayAnimationWithTransition(GetMovementAnimationName());

                Vector3 moveDirection = new Vector3(inputMovement.x, 0f, inputMovement.z);
                HumanController.Transform.Position += moveDirection * Time.DeltaTime * MovementSpeed;
                HumanController.Transform.Rotation = Matrix4x4.LookRotation(Vector3.Lerp(HumanController.Transform.Forward, moveDirection, Time.DeltaTime * 15f), Vector3.up).GetEulerRotation();
            }
            else
            {
                HumanController.HumanAnimator.PlayAnimationWithTransition(GetIdleAnimationName());
            }
        }
    }
}
