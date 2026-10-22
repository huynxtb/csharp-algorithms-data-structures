using System;
using System.Collections.Generic;

public class CyclicBuffer<T>
{
    private T[] buffer;
    private int head;
    private int tail;
    private int count;
    private int capacity;

    public CyclicBuffer(int size)
    {
        if (size <= 0)
            throw new ArgumentOutOfRangeException("size", "Buffer size must be a positive integer.");
        capacity = size;
        buffer = new T[capacity];
        head = 0;
        tail = 0;
        count = 0;
    }

    public void Add(T item)
    {
        buffer[tail] = item;
        tail = (tail + 1) % capacity;
        if (IsFull)
        {
            head = (head + 1) % capacity; // Overwrite the oldest data
        }
        else
        {
            count++;
        }
    }

    public T Get(int index)
    {
        if (index < 0 || index >= count)
            throw new ArgumentOutOfRangeException("index", "Index is out of range.");
        return buffer[(head + index) % capacity];
    }

    public int Count
    {
        get { return count; }
    }

    public bool IsFull
    {
        get { return count == capacity; }
    }
}