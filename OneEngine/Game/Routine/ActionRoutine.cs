using System;

namespace OneEngine
{
    public sealed class ActionRoutine : IRoutine
    {
        Action action;

        public ActionRoutine(Action action)
        {
            this.action = action;
        }

        public bool NextStep()
        {
            action();
            return true;
        }
    }
}