using System.Collections;

namespace RomeEngine
{
    public sealed class EnumeratorRoutine : IRoutine
    {
        IEnumerator enumerator;

        public EnumeratorRoutine(IEnumerator enumerator)
        {
            this.enumerator = enumerator;
        }

        public bool NextStep()
        {
            return enumerator.MoveNext();
        }
    }
}