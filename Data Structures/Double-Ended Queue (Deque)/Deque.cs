using System;

public class Deque<T>
{
    private T[] _array;
    private int _head;
    private int _tail;
    private int _count;
    private const int DefaultCapacity = 8;

    public Deque()
    {
        _array = new T[DefaultCapacity];
        _head = 0;
        _tail = 0;
        _count = 0;
    }

    public int Count => _count;

    public bool IsEmpty => _count == 0;

    public void AddFront(T item)
    {
        if (_count == _array.Length)
        { 
            Resize(_array.Length * 2);
        }

        _head = (_head - 1 + _array.Length) % _array.Length;
        _array[_head] = item;
        _count++;
    }

    public void AddRear(T item)
    {
        if (_count == _array.Length)
        { 
            Resize(_array.Length * 2);
        }

        _array[_tail] = item;
        _tail = (_tail + 1) % _array.Length;
        _count++;
    }

    public T RemoveFront()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("The deque is empty.");
        }

        T item = _array[_head];
        _array[_head] = default;
        _head = (_head + 1) % _array.Length;
        _count--;
        return item;
    }

    public T RemoveRear()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("The deque is empty.");
        }

        _tail = (_tail - 1 + _array.Length) % _array.Length;
        T item = _array[_tail];
        _array[_tail] = default;
        _count--;
        return item;
    }

    public T PeekFront()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("The deque is empty.");
        }

        return _array[_head];
    }

    public T PeekRear()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("The deque is empty.");
        }

        int index = (_tail - 1 + _array.Length) % _array.Length;
        return _array[index];
    }

    private void Resize(int newCapacity)
    {
        T[] newArray = new T[newCapacity];
        for (int i = 0; i < _count; i++)
        { 
            newArray[i] = _array[(_head + i) % _array.Length];
        }

        _array = newArray;
        _head = 0;
        _tail = _count;
    }
}