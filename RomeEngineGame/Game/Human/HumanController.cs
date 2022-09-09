﻿using RomeEngine;
using System.Linq;

namespace OneEngineGame
{
    public abstract class HumanController : Component
    {
        protected HumanStateMachine StateMachine { get; private set; }
        public HumanAnimator HumanAnimator { get; private set; }

        [BehaviourEvent]
        void Start()
        {
            HumanAnimator = GameObject.AddComponent<HumanAnimator>();
            HumanAnimator.PlaybackSpeed = 1f;
            StateMachine = new HumanStateMachine();
            StateMachine.Initialize(this);
        }

        protected override void OnEventCall(string name)
        {
            base.OnEventCall(name);
            StateMachine.CallEvent(name);
        }

        public abstract IControlAgent GetControlAgent();
    }
}
