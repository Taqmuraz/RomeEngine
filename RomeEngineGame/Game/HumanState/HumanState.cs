using RomeEngine;

namespace RomeEngineEditor
{
    public abstract class HumanState : EventsHandler, IState<string>
    {
        string name;

        public HumanState()
        {
            name = GetType().Name.Replace("Human", string.Empty).Replace("State", string.Empty);
        }

        protected HumanController HumanController { get; private set; }

        protected virtual string GetName() => name;
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
