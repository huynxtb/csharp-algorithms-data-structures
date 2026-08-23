using System;

public class ArrayStack<T>
{
    private T[] _items;
    private int _size;
    private const int DefaultCapacity = 4;

    public ArrayStack(int capacity = DefaultCapacity)
    { 
        if (capacity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be non-negative.");
        }
        _items = new T[capacity];
        _size = 0;
    }

    public int Count => _size;

    public bool IsEmpty => _size == 0;

    public void Push(T item)
    {
        if (_size == _items.Length)
        {
            Resize(_items.Length == 0 ? DefaultCapacity : _items.Length * 2);
        }
        _items[_size++] = item;
    }

    public T Pop()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Stack is empty.");
        }
        T item = _items[--_size];
        _items[_size] = default;
        return item;
    }

    public T Peek()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Stack is empty.");
        }
        return _items[_size - 1];
    }

    private void Resize(int newCapacity)
    {
        T[] newArray = new T[newCapacity];
        Array.Copy(_items, newArray, _size);
        _items = newArray;
    }
}