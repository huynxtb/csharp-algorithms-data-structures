using System;

public class FixedCapacityArrayQueue<T>
{
    private T[] _items;
    private int _count;
    private readonly int _capacity;

    public FixedCapacityArrayQueue(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be a positive integer.");
        }
        _capacity = capacity;
        _items = new T[_capacity];
        _count = 0;
    }

    public int Count
    {
        get { return _count; }
    }

    public int Capacity
    {
        get { return _capacity; }
    }

    public bool IsEmpty()
    {
        return _count == 0;
    }

    public bool IsFull()
    {
        return _count == _capacity;
    }

    public void Enqueue(T item)
    {
        if (IsFull())
        {
            throw new InvalidOperationException("Queue is full.");
        }

        _items[_count] = item;
        _count++;
    }

    public T Dequeue()
    {
        if (IsEmpty())
        {
            throw new InvalidOperationException("Queue is empty.");
        }

        T dequeuedItem = _items[0];

        // Shift elements forward
        for (int i = 0; i < _count - 1; i++)
        {
            _items[i] = _items[i + 1];
        }

        // Clear the last element's old position
        _items[_count - 1] = default(T);
        _count--;

        return dequeuedItem;
    }

    public T Peek()
    {
        if (IsEmpty())
        {
            throw new InvalidOperationException("Queue is empty.");
        }
        return _items[0];
    }
}