using System.Collections;
using System.Collections.Generic;

namespace RomeEngine
{
    public sealed class Routine : Component
    {
        IRoutine routine;

        static List<IRoutine> delayedRoutines = new List<IRoutine>();

        public static void StartRoutineDelayed(IRoutine routine)
        {
            lock (delayedRoutines)
            {
                delayedRoutines.Add(routine);
            }
        }

        public static void UpdateDelayed()
        {
            lock (delayedRoutines)
            {
                foreach (var routine in delayedRoutines) StartRoutine(routine);
                delayedRoutines.Clear();
            }
        }

        public static Routine StartRoutine(IRoutine routine)
        {
            if (routine == null) throw new System.ArgumentNullException(nameof(routine));
            var instance = new GameObject("Routine").ActivateForActiveScene().AddComponent<Routine>();
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
            GameObject.Deactivate(GameScene.ActiveScene);
        }
        [BehaviourEvent]
        void Update()
        {
            if (routine != null)
            {
                if (!routine.NextStep())
                {
                    routine = null;
                    GameObject.Deactivate(GameScene.ActiveScene);
                }
            }
        }
    }
}