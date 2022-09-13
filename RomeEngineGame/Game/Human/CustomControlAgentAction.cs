using System;

namespace OneEngineGame
{
    public sealed class CustomControlAgentAction : IControlAgentAction
    {
        Action<IControlAgentActor> action;

        public CustomControlAgentAction(Action<IControlAgentActor> action)
        {
            this.action = action;
        }

        void IControlAgentAction.AcceptActor(IControlAgentActor actor)
        {
            action(actor);
        }
    }
}
