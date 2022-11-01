using RomeEngine;

namespace RomeEngineEditor
{
    public sealed class HumanShieldStrikeState : HumanAnimatedConstantMoveState
    {
        protected override string GetExitStateName() => "SwordDefault";
        protected override Vector2 GetVelocity() => new Vector2();//NormalizedTime < 0.5f ? HumanController.Transform.LocalRight * 3f : Vector2.zero;
        protected override string GetAnimationName() => "Shield_Strike";
        protected override float TimeLength => 0.7f;
    }
}
