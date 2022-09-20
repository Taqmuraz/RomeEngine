using RomeEngine;

namespace RomeEngineGame
{
    public sealed class HumanShieldStrikeState : HumanAnimatedConstantMoveState
    {
        protected override string GetExitStateName() => "SwordDefault";
        protected override Vector2 GetVelocity() => NormalizedTime < 0.5f ? HumanController.Transform.LocalRight * 3f : Vector2.zero;
        protected override string GetAnimationName() => "Shield_Strike";
        protected override float TimeLength => 0.7f;
    }
}
