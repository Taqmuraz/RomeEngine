using System.Collections.Generic;

namespace RomeEngine.IO
{
    public abstract class ColladaStackContainingObject<TElement>
    {
        List<TElement> elementsPassed = new List<TElement>();
        Stack<TElement> elementsStack = new Stack<TElement>();
        protected IEnumerable<TElement> Elements => elementsPassed;

        protected TElement CurrentElement => elementsStack.Peek();

        public void PushElement(TElement element)
        {
            elementsStack.Push(element);
        }
        public void PopElement()
        {
            elementsPassed.Add(elementsStack.Pop());
        }
    }
}
