using RomeEngine;

namespace OneEngineGame
{
    public abstract class HumanState : EventsHandler, IState<string>
    {
        protected HumanController HumanController { get; private set; }

        protected abstract string GetName();
        protected abstract string GetNextStateName();
        string IState<string>.GetStateKey() => GetName();
        string IState<string>.GetNextStateKey() => GetNextStateName();

        public HumanState Initialize(HumanController human)
        {
            HumanController = human;
            return this;
        }
    }
}
