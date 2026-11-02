using System;

public class PriorityQueueSortedArray<T> where T : IComparable<T>
{
    private T[] _elements;
    private int _count;
    private const int DefaultCapacity = 4;

    public PriorityQueueSortedArray()
    {
        _elements = new T[DefaultCapacity];
        _count = 0;
    }

    public PriorityQueueSortedArray(int initialCapacity)
    {
        if (initialCapacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Initial capacity must be positive.");
        }
        _elements = new T[initialCapacity];
        _count = 0;
    }

    public int Count
    {
        get { return _count; }
    }

    public bool IsEmpty()
    {
        return _count == 0;
    }

    public void Insert(T element)
    {
        if (element == null)
        {
            throw new ArgumentNullException(nameof(element), "Element cannot be null.");
        }

        EnsureCapacity();

        int insertIndex = 0;
        while (insertIndex < _count && _elements[insertIndex].CompareTo(element) < 0)
        {
            insertIndex++;
        }

        ShiftElements(insertIndex, 1);
        _elements[insertIndex] = element;
        _count++;
    }

    public T RemoveMax()
    {
        if (IsEmpty())
        {
            throw new InvalidOperationException("Priority queue is empty.");
        }

        T maxElement = _elements[_count - 1];
        _elements[_count - 1] = default(T);
        _count--;
        return maxElement;
    }

    public T PeekMax()
    {
        if (IsEmpty())
        {
            throw new InvalidOperationException("Priority queue is empty.");
        }
        return _elements[_count - 1];
    }

    private void EnsureCapacity()
    {
        if (_count == _elements.Length)
        {
            Resize(_elements.Length * 2);
        }
    }

    private void Resize(int newCapacity)
    {
        T[] newArray = new T[newCapacity];
        Array.Copy(_elements, newArray, _count);
        _elements = newArray;
    }

    private void ShiftElements(int startIndex, int shiftAmount)
    {
        if (startIndex < 0 || startIndex > _count)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex));
        }
        if (shiftAmount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(shiftAmount), "Shift amount cannot be negative.");
        }

        // Shift elements to the right to make space for insertion
        for (int i = _count - 1; i >= startIndex; i--)
        {
            _elements[i + shiftAmount] = _elements[i];
        }
    }
}