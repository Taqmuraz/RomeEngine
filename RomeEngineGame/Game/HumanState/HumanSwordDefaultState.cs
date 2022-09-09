using RomeEngine;

namespace OneEngineGame
{
    public sealed class HumanSwordDefaultState : HumanMovementState
    {
        protected override float MovementSpeed => 5f;

        protected override string GetNextStateName()
        {
            return GetName();
        }
        protected override string GetMovementAnimationName() => "Sword_Run";
        protected override string GetIdleAnimationName() => "Sword_Idle";
    }
}
