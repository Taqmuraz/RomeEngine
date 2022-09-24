using System.Collections.Generic;

namespace RomeEngine.IO
{
    public class ColladaStackContainingObject<TElement>
    {
        List<TElement> elementsPassed = new List<TElement>();
        Stack<TElement> elementsStack = new Stack<TElement>();
        public ReadOnlyArrayList<TElement> Elements => elementsPassed;

        public TElement CurrentElement => elementsStack.Peek();

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
