using System;
using System.Collections.Generic;

public class SparseSet<T>
{
    private List<T> values;
    private List<int> indices;
    private int size;

    public SparseSet()
    {
        values = new List<T>();
        indices = new List<int>();
        size = 0;
    }

    public void Insert(int key, T value)
    {
        if (key >= indices.Count)
        {
            for (int i = indices.Count; i <= key; i++)
            {
                indices.Add(-1);
            }
        }

        if (indices[key] == -1)
        {
            indices[key] = size;
            values.Add(value);
            size++;
        }
        else
        {
            values[indices[key]] = value;
        }
    }

    public void Remove(int key)
    {
        if (key < indices.Count && indices[key] != -1)
        {
            int index = indices[key];
            indices[key] = -1;
            values[index] = default(T);
        }
    }

    public T Get(int key)
    {
        if (key < indices.Count && indices[key] != -1)
        {
            return values[indices[key]];
        }
        throw new KeyNotFoundException("Key not found.");
    }
}