using System;
using System.Collections;
using System.Collections.Generic;

public sealed class ReadOnlyArray<T> : IEnumerable<T>, IList
{
	T[] array;

	public int Length => array == null ? 0 : array.Length;

	public ReadOnlyArray(T[] array)
	{
		this.array = array;
	}
    public ReadOnlyArray(Array array)
    {
        this.array = (T[])array;
    }

	public T this[int index]
	{
		get
		{
			if (array == null) throw new System.IndexOutOfRangeException();
			else return array[index];
		}
	}

	public IEnumerator<T> EmptyEnumerator()
	{
		yield break;
	}

	public IEnumerator<T> GetEnumerator()
	{
		return array == null ? EmptyEnumerator() : ((IEnumerable<T>)array).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return array == null ? EmptyEnumerator() : array.GetEnumerator();
	}

	public static implicit operator ReadOnlyArray<T>(T[] array)
	{
		return new ReadOnlyArray<T>(array);
	}
	public static implicit operator ReadOnlyArray<T>(Array array)
	{
		return new ReadOnlyArray<T>((T[])array);
	}

	public void CopyTo(Array array, int index)
    {
        this.array.CopyTo(array, index);
    }

    public int Count => ((ICollection)array).Count;

    public object SyncRoot => array.SyncRoot;

    public bool IsSynchronized => array.IsSynchronized;

    public int Add(object value)
    {
        throw new NotImplementedException();
    }

    public bool Contains(object value)
    {
        return ((IList)array).Contains(value);
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public int IndexOf(object value)
    {
        return ((IList)array).IndexOf(value);
    }

    public void Insert(int index, object value)
    {
        throw new NotImplementedException();
    }

    public void Remove(object value)
    {
        throw new NotImplementedException();
    }

    public void RemoveAt(int index)
    {
        throw new NotImplementedException();
    }

    object IList.this[int index] { get => array[index]; set => array[index] = (T)value; }

    public bool IsReadOnly => array.IsReadOnly;

    public bool IsFixedSize => array.IsFixedSize;
}