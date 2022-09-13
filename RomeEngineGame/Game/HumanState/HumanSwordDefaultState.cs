using RomeEngine;

namespace OneEngineGame
{
    public sealed class HumanSwordDefaultState : HumanMovementState, IControlAgentActor
    {
        protected override float MovementSpeed => 5f;
        string nextState;

        [BehaviourEvent]
        void OnEnter()
        {
            nextState = GetName();
        }
        [BehaviourEvent]
        void Update()
        {
            HumanController.GetControlAgent().GetAction().AcceptActor(this);
        }

        protected override string GetNextStateName()
        {
            return nextState;
        }
        protected override string GetMovementAnimationName() => "Sword_Run";
        protected override string GetIdleAnimationName() => "Sword_Idle";

        void IControlAgentActor.MoveToState(string stateName)
        {
            nextState = stateName;
        }
    }
}
