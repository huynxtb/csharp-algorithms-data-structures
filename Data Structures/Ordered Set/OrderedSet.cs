using System;
using System.Collections.Generic;
using System.Linq;

public class OrderedSet<T> where T : IComparable<T>
{
    private readonly SortedSet<T> _set;

    public OrderedSet()
    {
        _set = new SortedSet<T>();
    }

    public void Add(T element)
    {
        _set.Add(element);
    }

    public bool Remove(T element)
    {
        return _set.Remove(element);
    }

    public bool Contains(T element)
    {
        return _set.Contains(element);
    }

    public T[] GetSortedElements()
    {
        return _set.ToArray();
    }
}