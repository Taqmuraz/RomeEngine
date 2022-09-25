using System;
using System.Collections.Generic;

public static class CollectionsExtensions
{
	public static IEnumerable<TElement> TraceElement<TElement>(this TElement element, Func<TElement, IEnumerable<TElement>> childNodes)
	{
		List<TElement> trace = new List<TElement>();
		TraceElement(element, childNodes, trace);
		return trace;
	}
	static void TraceElement<TElement>(TElement element, Func<TElement, IEnumerable<TElement>> childNodes, List<TElement> trace)
	{
		if (trace.Contains(element)) return;
		trace.Add(element);
		foreach (var child in childNodes(element)) TraceElement(child, childNodes, trace);
	}
    public static TResult Iterate<TElement, TResult>(this TElement element, TResult seed, Func<TElement, TResult, TResult> iteration, Func<TElement, (TElement element, bool hasNext)> nextElement)
    {
        seed = iteration(element, seed);
        var next = nextElement(element);
        if (next.hasNext)
        {
            return Iterate(next.element, seed, iteration, nextElement);
        }
        else return seed;
    }
	public static int IndexOf<T>(this IEnumerable<T> collection, T element)
	{
		int index = 0;
		foreach (var c in collection)
		{
			if (c.Equals(element)) return index;
			index++;
		}
		return -1;
	}
}
