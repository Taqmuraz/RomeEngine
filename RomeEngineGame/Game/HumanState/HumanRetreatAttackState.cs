using RomeEngine;

namespace RomeEngineGame
{
    public sealed class HumanRetreatAttackState : HumanAnimatedConstantMoveState
    {
        protected override string GetExitStateName() => "SwordDefault";
        protected override Vector2 GetVelocity() => -HumanController.Transform.LocalRight * 4f;
        protected override string GetAnimationName() => "SwordAttack_Retreat";
        protected override float TimeLength => 1f;
    }
}
