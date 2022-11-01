using System.Collections.Generic;

public interface IBuffer<TElement>
{
	bool Write(TElement element);
	void Reset();
	int Position { get; }

	IEnumerable<TElement> Enumerate();
	TElement[] ToArray();
}
