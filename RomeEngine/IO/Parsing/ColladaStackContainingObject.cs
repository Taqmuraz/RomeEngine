using System.Collections.Generic;

namespace RomeEngine.IO
{
    public class ColladaStackContainingObject<TElement>
    {
        List<TElement> elementsPassed = new List<TElement>();
        Stack<TElement> elementsStack = new Stack<TElement>();
        public ReadOnlyArrayList<TElement> Elements => elementsPassed;

        public int StackDepth => elementsStack.Count;

        public TElement CurrentElement => elementsStack.Peek();

        public void PushElement(TElement element)
        {
            elementsStack.Push(element);
        }
        public TElement PopElement()
        {
            var pop = elementsStack.Pop();
            elementsPassed.Add(pop);
            return pop;
        }
    }
}
