using System;

namespace RomeEngine
{
    public sealed class SingleCallRoutine : IRoutine
    {
        Action action;

        public SingleCallRoutine(Action action)
        {
            this.action = action;
        }

        public bool NextStep()
        {
            action();
            return false;
        }
    }
}