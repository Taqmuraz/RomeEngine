using System.Collections;

namespace OneEngine
{
    public sealed class Routine : Component
    {
        IRoutine routine;

        public static Routine StartRoutine(IRoutine routine)
        {
            if (routine == null) throw new System.ArgumentNullException(nameof(routine));
            var instance = new GameObject("Routine").AddComponent<Routine>();
            instance.routine = routine;
            return instance;
        }

        public static Routine StartRoutine(IEnumerator routine)
        {
            if (routine == null) throw new System.ArgumentNullException(nameof(routine));
            return StartRoutine(new EnumeratorRoutine(routine));
        }

        public void Stop()
        {
            GameObject.Destroy();
        }
        [BehaviourEvent]
        void Update()
        {
            if (routine != null) if (!routine.NextStep()) routine = null;
        }
    }
}