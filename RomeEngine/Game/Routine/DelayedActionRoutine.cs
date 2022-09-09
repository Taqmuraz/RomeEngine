using System;

namespace RomeEngine
{
    public sealed class DelayedActionRoutine : IRoutine
    {
        Action action;
        float delay;
        float startTime;

        public DelayedActionRoutine(Action action, float delay)
        {
            this.action = action;
            this.delay = delay;
            startTime = Time.CurrentTime;
        }

        public bool NextStep()
        {
            if (Time.CurrentTime < startTime + delay) return true;
            else
            {
                action();
                return false;
            }
        }
    }
}