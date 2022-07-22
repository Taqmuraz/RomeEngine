using System;
using System.Collections;
using System.Collections.Generic;

public sealed class SafeDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
{
    private Dictionary<TKey, TValue> provider;
	private Func<TValue> defaultValueFunc;

	public SafeDictionary(Func<TValue> defaultValueFunc = null)
	{
		provider = new Dictionary<TKey, TValue>();
		if (defaultValueFunc == null) defaultValueFunc = () => default(TValue);
		this.defaultValueFunc = defaultValueFunc;
	}

	public TValue this[TKey key]
	{
		get
		{
			if (key == null) return defaultValueFunc();
			TValue value;
			if (provider.TryGetValue(key, out value))
			{
				return value;
			}
			else
			{
				value = defaultValueFunc();
				provider.Add(key, value);
				return value;
			}
		}
		set
		{
			provider[key] = value;
		}
	}

	public void Add(TKey key, TValue value)
	{
		this[key] = value;
	}
	public void Remove(TKey key)
	{
		if (provider.ContainsKey(key)) provider.Remove(key);
	}

	public bool ContainsKey(TKey key)
	{
		if (key == null) return false;
		else return provider.ContainsKey(key);
	}

	public void SetDefaultValueFunc (Func<TValue> defaultInstanceFunc)
	{
		if (defaultInstanceFunc != null)
			this.defaultValueFunc = defaultInstanceFunc;
		else defaultInstanceFunc = () => default(TValue);
	}

	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return provider.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return provider.GetEnumerator();
	}
}