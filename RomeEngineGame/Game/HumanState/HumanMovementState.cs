using RomeEngine;

namespace RomeEngineGame
{
    public abstract class HumanMovementState : HumanState
    {
        protected abstract string GetMovementAnimationName();
        protected abstract string GetIdleAnimationName();
        protected abstract float MovementSpeed { get; }

        [BehaviourEvent]
        void Update()
        {
            var inputMovement = HumanController.GetControlAgent().InputMovement.WithY(0f).normalized;
            if (inputMovement.x != 0)
            {
                HumanController.Transform.FlipY = inputMovement.x < 0f;
                HumanController.HumanAnimator.PlayAnimationWithTransition(GetMovementAnimationName());
            }
            else
            {
                HumanController.HumanAnimator.PlayAnimationWithTransition(GetIdleAnimationName());
            }
            HumanController.Transform.Position += inputMovement * Time.DeltaTime * MovementSpeed;
        }
    }
}
