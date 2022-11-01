using System.Collections;
using RomeEngine;

namespace RomeEngineEditor
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