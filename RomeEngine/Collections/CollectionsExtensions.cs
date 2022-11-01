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

	public static TElement FindMin<TElement, TValue>(this IEnumerable<TElement> collection, Func<TElement, TValue> valueGetter) where TValue : IComparable
	{
		TValue min = default;
		bool hasMin = false;
		TElement minElement = default;

		foreach (var element in collection)
		{
			var value = valueGetter(element);
			if (!hasMin || value.CompareTo(min) < 0)
			{
				hasMin = true;
				min = value;
				minElement = element;
			}
		}
		return minElement;
	}
	public static TElement FindMax<TElement, TValue>(this IEnumerable<TElement> collection, Func<TElement, TValue> valueGetter) where TValue : IComparable
	{
		TValue max = default;
		bool hasMax = false;
		TElement maxElement = default;

		foreach (var element in collection)
		{
			var value = valueGetter(element);
			if (!hasMax || value.CompareTo(max) > 0)
			{
				hasMax = true;
				max = value;
				maxElement = element;
			}
		}
		return maxElement;
	}

	public static char[] StringSeparators { get; } = new char[] { ' ', '\n', '\r', '\t' };

	public static string[] SeparateString(this string source)
	{
		return source.Split(StringSeparators, StringSplitOptions.RemoveEmptyEntries);
	}
}
