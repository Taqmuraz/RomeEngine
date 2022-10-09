using System;
using System.Collections;
using System.Collections.Generic;

public sealed class DynamicLinkedList<T> : IList<T>
{
    sealed class DynamicLinkedListNode
    {
        public DynamicLinkedListNode(T value, DynamicLinkedListNode next)
        {
            Value = value;
            Next = next;
        }

        public T Value { get; set; }
        public DynamicLinkedListNode Next { get; set; }
    }

    DynamicLinkedListNode First { get; set; }

    public int IndexOf(T item)
    {
        int index = 0;
        var node = First;
        while(node != null && !node.Value.Equals(item))
        {
            index++;
            node = node.Next;
        }
        if (node == null) return -1;
        else return index;
    }

    public void Insert(int index, T item)
    {
        DynamicLinkedListNode node = First;
        CheckOutOfRange(node);
        for (int i = 0; i < index; i++)
        {
            node = node.Next;
            CheckOutOfRange(node);
        }
        node.Next = new DynamicLinkedListNode(node.Value, node.Next);
        node.Value = item;
    }

    public void RemoveAt(int index)
    {
        if (index == 0)
        {
            CheckOutOfRange(First);
            First = First.Next;
        }
        else
        {
            DynamicLinkedListNode node = First;
            CheckOutOfRange(node);
            for (int i = 0; i < index - 1; i++)
            {
                node = node.Next;
                CheckOutOfRange(node);
            }
            node.Next = node.Next.Next;
            node.Value = node.Next.Value;
        }
    }

    void CheckOutOfRange(DynamicLinkedListNode node)
    {
        if (node == null) throw new IndexOutOfRangeException();
    }

    public T this[int index]
    {
        get
        {
            DynamicLinkedListNode node = First;
            for (int i = 0; i < index; i++) node = node.Next;
            return node.Value;
        }
        set
        {
            DynamicLinkedListNode node = First;
            CheckOutOfRange(node);
            for (int i = 0; i < index; i++)
            {
                node = node.Next;
                CheckOutOfRange(node);
            }
            node.Value = value;
        }
    }

    public void Add(T item)
    {
        DynamicLinkedListNode last = First;
        if (last == null)
        {
            First = new DynamicLinkedListNode(item, null);
        }
        else
        {
            while (last.Next != null) last = last.Next;
            last.Next = new DynamicLinkedListNode(item, null);
        }
    }

    public void Clear()
    {
        First = null;
    }

    public bool Contains(T item)
    {
        DynamicLinkedListNode node = First;
        while (node != null)
        {
            if (node.Value.Equals(item)) return true;
            node = node.Next;
        }
        return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        DynamicLinkedListNode node = First;
        while (node != null)
        {
            array[arrayIndex++] = node.Value;
            node = node.Next;
        }
    }

    public bool Remove(T item)
    {
        DynamicLinkedListNode node = First;
        DynamicLinkedListNode prev = null;
        while (node != null)
        {
            if (node.Value.Equals(item))
            {
                if (node == First)
                {
                    First = node.Next;
                }
                else
                {
                    prev.Next = node.Next;
                }
                return true;
            }
            prev = node;
            node = node.Next;
        }
        return false;
    }

    public int Count
    {
        get
        {
            int count = 0;
            DynamicLinkedListNode node = First;
            while (node != null)
            {
                node = node.Next;
                count++;
            }
            return count;
        }
    }
    public bool IsReadOnly => false;

    public IEnumerator<T> GetEnumerator()
    {
        DynamicLinkedListNode node = First;
        while (node != null)
        {
            yield return node.Value;
            node = node.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
