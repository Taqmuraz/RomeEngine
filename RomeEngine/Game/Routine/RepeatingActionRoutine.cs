using System;

namespace RomeEngine
{
    public sealed class RepeatingActionRoutine : IRoutine
    {
        Action action;
        float deltaTime;
        float lastTime;

        public RepeatingActionRoutine(Action action, float deltaTime)
        {
            this.action = action;
            this.deltaTime = deltaTime;
        }

        public bool NextStep()
        {
            if (Time.CurrentTime >= lastTime + deltaTime)
            {
                lastTime = Time.CurrentTime;
                action();
            }
            return true;
        }
    }
}