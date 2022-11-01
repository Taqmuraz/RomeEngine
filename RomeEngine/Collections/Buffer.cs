using System.Collections.Generic;

public class Buffer<TElement> : IBuffer<TElement>
{
	TElement[] array;
	int position;

    public Buffer(TElement[] array)
    {
        this.array = array;
    }
	public Buffer(int size)
	{
		array = new TElement[size];
	}

    public bool Write(TElement element)
    {
		if (position < array.Length)
		{
			array[position++] = element;
			return true;
		}
		return false;
    }

	public TElement[] ToArray() => array;

    public void Reset()
    {
		position = 0;
    }

    public int Position => position;

    public IEnumerable<TElement> Enumerate()
    {
        for (int i = 0; i < position; i++)
        {
            yield return array[i];
        }
    }
}
