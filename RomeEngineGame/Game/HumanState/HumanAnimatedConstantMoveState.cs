﻿using RomeEngine;

namespace RomeEngineGame
{
    public abstract class HumanAnimatedConstantMoveState : HumanState
    {
        protected abstract string GetExitStateName();
        protected abstract string GetAnimationName();
        protected abstract float TimeLength { get; }
        protected abstract Vector2 GetVelocity();
        float startTime;

        protected float NormalizedTime => (Time.CurrentTime - startTime) / TimeLength;

        [BehaviourEvent]
        void OnEnter()
        {
            startTime = Time.CurrentTime;
            HumanController.HumanAnimator.PlayAnimation(GetAnimationName());
        }

        protected override string GetNextStateName()
        {
            if (startTime + TimeLength > Time.CurrentTime) return GetName();
            else return GetExitStateName();
        }

        [BehaviourEvent]
        void Update()
        {
            HumanController.Transform.Position += GetVelocity() * Time.DeltaTime;
        }
    }
}
