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
}
