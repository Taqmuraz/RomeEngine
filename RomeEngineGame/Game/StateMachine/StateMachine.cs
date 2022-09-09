using RomeEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OneEngineGame
{
    public abstract class StateMachine<TKey, TState> : EventsHandler
        where TState : IState<TKey>
        where TKey : IComparable<TKey>
    {
        SafeDictionary<TKey, TState> statesMap;
        public TState ActiveState { get; private set; }

        protected abstract IEnumerable<TState> CreateStates();

        protected void Initialize()
        {
            var states = CreateStates().ToArray();
            var defaultState = states[0];
            statesMap = new SafeDictionary<TKey, TState>(states.ToDictionary(s => s.GetStateKey()), () => defaultState);
            ActiveState = defaultState;
            ActiveState.CallEvent("OnEnter");
        }

        protected override void OnEventCall(string name)
        {
            if (ActiveState != null) ActiveState.CallEvent(name);
        }

        [BehaviourEvent]
        void Update()
        {
            var nextKey = ActiveState.GetNextStateKey();
            if (nextKey.CompareTo(ActiveState.GetStateKey()) != 0)
            {
                ActiveState.CallEvent("OnExit");
                ActiveState = statesMap[nextKey];
                ActiveState.CallEvent("OnEnter");
            }
        }
    }
}
