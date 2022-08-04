using System.Collections;

namespace OneEngine
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