using RomeEngine;

namespace RomeEngineGame
{
    public sealed class HumanAirAttackState : HumanAnimatedConstantMoveState
    {
        protected override string GetExitStateName() => "SwordDefault";
        protected override Vector2 GetVelocity() => Vector2.zero;
        protected override string GetAnimationName() => "SwordAttack_AirStrike";
        protected override float TimeLength => 1f;
    }
}
