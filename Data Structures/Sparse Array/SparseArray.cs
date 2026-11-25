using System;
using System.Collections.Generic;

public class SparseArray<T>
{
    private Dictionary<int, T> elements;
    private int size;
    private T defaultValue;

    public SparseArray(int size, T defaultValue = default(T))
    {
        this.size = size;
        this.defaultValue = defaultValue;
        elements = new Dictionary<int, T>();
    }

    public void Set(int index, T value)
    {
        if (index < 0 || index >= size)
            throw new IndexOutOfRangeException("Index is out of bounds.");
        if (!EqualityComparer<T>.Default.Equals(value, defaultValue))
        {
            elements[index] = value;
        }
        else
        {
            elements.Remove(index);
        }
    }

    public T Get(int index)
    {
        if (index < 0 || index >= size)
            throw new IndexOutOfRangeException("Index is out of bounds.");
        return elements.TryGetValue(index, out T value) ? value : defaultValue;
    }

    public int CountNonDefaultElements()
    {
        return elements.Count;
    }

    public T[] ToArray()
    {
        T[] fullArray = new T[size];
        foreach (var kvp in elements)
        {
            fullArray[kvp.Key] = kvp.Value;
        }
        return fullArray;
    }
}