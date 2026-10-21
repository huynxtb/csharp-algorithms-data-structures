using System;

public class PriorityQueueSortedArray
{
    private int[] _array;
    private int _count;
    private const int DefaultCapacity = 4;

    public PriorityQueueSortedArray()
    {
        _array = new int[DefaultCapacity];
        _count = 0;
    }

    public PriorityQueueSortedArray(int initialCapacity)
    {
        if (initialCapacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Initial capacity must be greater than zero.");

        _array = new int[initialCapacity];
        _count = 0;
    }

    public int Count => _count;

    public bool IsEmpty()
    {
        return _count == 0;
    }

    public void Enqueue(int element)
    {
        if (_count == _array.Length)
        {
            Resize(_array.Length * 2);
        }

        // Find the correct position to insert the element to maintain sorted order (ascending).
        // Elements are stored in ascending order so that the highest priority (largest) element
        // is at the end of the array for O(1) removal.
        int insertIndex = FindInsertPosition(element);

        // Shift elements to the right to make room for the new element.
        for (int i = _count; i > insertIndex; i--)
        {
            _array[i] = _array[i - 1];
        }

        _array[insertIndex] = element;
        _count++;
    }

    public int Dequeue()
    {
        if (IsEmpty())
            throw new InvalidOperationException("The priority queue is empty.");

        // The highest priority element (largest) is at the end of the sorted array.
        int highestPriority = _array[_count - 1];
        _count--;

        // Shrink the array if it is too large relative to the count.
        if (_count > 0 && _count <= _array.Length / 4)
        {
            Resize(_array.Length / 2);
        }

        return highestPriority;
    }

    public int Peek()
    {
        if (IsEmpty())
            throw new InvalidOperationException("The priority queue is empty.");

        // The highest priority element (largest) is at the end of the sorted array.
        return _array[_count - 1];
    }

    private int FindInsertPosition(int element)
    {
        // Binary search to find the correct insertion position.
        int low = 0;
        int high = _count - 1;

        while (low <= high)
        {
            int mid = low + (high - low) / 2;

            if (_array[mid] <= element)
            {
                low = mid + 1;
            }
            else
            {
                high = mid - 1;
            }
        }

        return low;
    }

    private void Resize(int newCapacity)
    {
        if (newCapacity < DefaultCapacity)
            newCapacity = DefaultCapacity;

        int[] newArray = new int[newCapacity];
        for (int i = 0; i < _count; i++)
        {
            newArray[i] = _array[i];
        }
        _array = newArray;
    }

    public int[] ToArray()
    {
        int[] result = new int[_count];
        for (int i = 0; i < _count; i++)
        {
            result[i] = _array[i];
        }
        return result;
    }
}