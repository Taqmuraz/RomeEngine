using RomeEngine;

namespace RomeEngineGame
{
    public abstract class HumanControlActorState : HumanState, IControlActor
    {
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

        protected  override string GetNextStateName()
        {
            return nextState;
        }

        void IControlActor.MoveToState(string stateName)
        {
            nextState = stateName;
        }
    }
}
