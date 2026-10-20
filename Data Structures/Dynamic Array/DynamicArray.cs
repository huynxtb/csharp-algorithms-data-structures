using System;

public class DynamicArray<T>
{
    private const int DEFAULT_CAPACITY = 4;
    private T[] _items;
    private int _count;

    public int Count
    {
        get { return _count; }
    }

    public int Capacity
    {
        get { return _items.Length; }
    }

    public DynamicArray()
    {
        _items = new T[DEFAULT_CAPACITY];
        _count = 0;
    }

    public DynamicArray(int initialCapacity)
    {
        if (initialCapacity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Initial capacity cannot be negative.");
        }
        _items = new T[initialCapacity == 0 ? DEFAULT_CAPACITY : initialCapacity];
        _count = 0;
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
            {
                throw new IndexOutOfRangeException("Index is out of range.");
            }
            return _items[index];
        }
        set
        {
            if (index < 0 || index >= _count)
            {
                throw new IndexOutOfRangeException("Index is out of range.");
            }
            _items[index] = value;
        }
    }

    public void Add(T item)
    {
        if (_count == Capacity)
        {
            Resize(_count == 0 ? 1 : Capacity * 2);
        }
        _items[_count++] = item;
    }

    public T RemoveAt(int index)
    {
        if (index < 0 || index >= _count)
        {
            throw new IndexOutOfRangeException("Index is out of range.");
        }

        T removedItem = _items[index];
        _count--;

        // Shift elements to the left
        for (int i = index; i < _count; i++)
        {
            _items[i] = _items[i + 1];
        }

        // Clear the last element's position to help GC
        _items[_count] = default(T);

        // Shrink capacity if necessary
        if (Capacity > DEFAULT_CAPACITY && _count <= Capacity / 4)
        {
            Resize(Capacity / 2);
        }

        return removedItem;
    }

    public void Clear()
    {
        // Option 1: Just reset count and clear elements
        for (int i = 0; i < _count; i++)
        {
            _items[i] = default(T);
        }
        _count = 0;

        // Option 2: Reset to default capacity and reallocate (more memory efficient if Clear is followed by many Adds)
        // _items = new T[DEFAULT_CAPACITY];
        // _count = 0;
    }

    private void Resize(int newCapacity)
    {
        T[] newArray = new T[newCapacity];
        Array.Copy(_items, newArray, _count);
        _items = newArray;
    }
}