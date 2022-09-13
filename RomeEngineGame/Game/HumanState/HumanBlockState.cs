using RomeEngine;

namespace OneEngineGame
{
    public abstract class HumanBlockState : HumanControlActorState
    {
        protected abstract string GetBlockAnimation();

        [BehaviourEvent]
        void OnEnter()
        {
            HumanController.HumanAnimator.PlayAnimationWithTransition(GetBlockAnimation());
        }
        [BehaviourEvent]
        void Update()
        {
            Vector2 input = HumanController.GetControlAgent().InputMovement;
            if (input.x != 0f) HumanController.Transform.FlipY = input.x > 0 ? false : true;
        }
    }
}
