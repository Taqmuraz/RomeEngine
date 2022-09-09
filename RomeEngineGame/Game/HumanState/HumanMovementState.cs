using RomeEngine;

namespace OneEngineGame
{
    public abstract class HumanMovementState : HumanState
    {
        string name;

        protected abstract float MovementSpeed { get; }

        public HumanMovementState()
        {
            name = GetType().Name.Replace("Human", string.Empty).Replace("State", string.Empty);
        }

        protected override string GetName() => name;
        protected abstract string GetMovementAnimationName();
        protected abstract string GetIdleAnimationName();

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
