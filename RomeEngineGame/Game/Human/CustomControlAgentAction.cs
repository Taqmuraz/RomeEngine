using System;

namespace OneEngineGame
{
    public sealed class CustomControlAgentAction : IControlAction
    {
        Action<IControlActor> action;

        public CustomControlAgentAction(Action<IControlActor> action)
        {
            this.action = action;
        }

        void IControlAction.AcceptActor(IControlActor actor)
        {
            action(actor);
        }
    }
}
