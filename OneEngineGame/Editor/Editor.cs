using System.Collections;
using OneEngine;

namespace OneEngineGame
{
    public abstract class Editor : Component
    {
        IEnumerator routine;

        [BehaviourEvent]
        void Start()
        {
            routine = Routine();
        }
        [BehaviourEvent]
        void Update()
        {
            if (routine != null && !routine.MoveNext())
            {
                routine = null;
            }
        }

        protected abstract IEnumerator Routine();
    }
}