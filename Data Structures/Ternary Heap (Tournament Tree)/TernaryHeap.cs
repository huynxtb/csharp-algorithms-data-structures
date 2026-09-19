using System;
using System.Collections.Generic;

/// <summary>
/// A generic Ternary Min-Heap (Tournament Tree) implementation where each node has up to three children.
/// Supports efficient priority queue operations with O(log_3 n) complexity for insertion and extraction.
/// </summary>
/// <typeparam name="T">The type of elements in the heap. Must implement IComparable&lt;T&gt;.</typeparam>
public class TernaryHeap<T> where T : IComparable<T>
{
    private readonly List<T> _items;

    /// <summary>
    /// Initializes a new empty instance of the <see cref="TernaryHeap{T}"/> class.
    /// </summary>
    public TernaryHeap()
    {
        _items = new List<T>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TernaryHeap{T}"/> class with a specified initial capacity.
    /// </summary>
    /// <param name="capacity">The initial capacity of the underlying storage.</param>
    public TernaryHeap(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be non-negative.");
        _items = new List<T>(capacity);
    }

    /// <summary>
    /// Gets the number of elements currently stored in the heap.
    /// </summary>
    public int Count => _items.Count;

    /// <summary>
    /// Gets a value indicating whether the heap contains no elements.
    /// </summary>
    public bool IsEmpty => _items.Count == 0;

    /// <summary>
    /// Inserts a new element into the heap while maintaining the min-heap property.
    /// </summary>
    /// <param name="item">The item to insert.</param>
    public void Insert(T item)
    {
        _items.Add(item);
        HeapifyUp(_items.Count - 1);
    }

    /// <summary>
    /// Returns the minimum element in the heap without removing it.
    /// </summary>
    /// <returns>The minimum element.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the heap is empty.</exception>
    public T PeekMin()
    {
        if (IsEmpty)
            throw new InvalidOperationException("The heap is empty.");
        return _items[0];
    }

    /// <summary>
    /// Removes and returns the minimum element from the heap.
    /// </summary>
    /// <returns>The minimum element.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the heap is empty.</exception>
    public T ExtractMin()
    {
        if (IsEmpty)
            throw new InvalidOperationException("The heap is empty.");

        T min = _items[0];
        int lastIndex = _items.Count - 1;

        if (lastIndex == 0)
        {
            _items.RemoveAt(0);
            return min;
        }

        _items[0] = _items[lastIndex];
        _items.RemoveAt(lastIndex);
        HeapifyDown(0);

        return min;
    }

    /// <summary>
    /// Decreases the value at a specific index and restores the heap property.
    /// The new value must be less than or equal to the current value at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to modify.</param>
    /// <param name="newValue">The new value which must be smaller than the current value.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is invalid.</exception>
    /// <exception cref="ArgumentException">Thrown when the new value is greater than the existing value.</exception>
    public void DecreaseKey(int index, T newValue)
    {
        if (index < 0 || index >= _items.Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

        if (Compare(newValue, _items[index]) > 0)
            throw new ArgumentException("New value is greater than the current value.", nameof(newValue));

        _items[index] = newValue;
        HeapifyUp(index);
    }

    /// <summary>
    /// Removes all elements from the heap.
    /// </summary>
    public void Clear()
    {
        _items.Clear();
    }

    /// <summary>
    /// Restores the heap property by moving the element at the specified index upward.
    /// </summary>
    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = GetParentIndex(index);
            if (Compare(_items[index], _items[parentIndex]) < 0)
            {
                Swap(index, parentIndex);
                index = parentIndex;
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// Restores the heap property by moving the element at the specified index downward.
    /// </summary>
    private void HeapifyDown(int index)
    {
        int count = _items.Count;
        while (true)
        {
            int smallest = index;
            int firstChild = 3 * index + 1;

            for (int i = 0; i < 3; i++)
            {
                int childIndex = firstChild + i;
                if (childIndex < count && Compare(_items[childIndex], _items[smallest]) < 0)
                {
                    smallest = childIndex;
                }
            }

            if (smallest == index)
                break;

            Swap(index, smallest);
            index = smallest;
        }
    }

    /// <summary>
    /// Calculates the parent index of a given node in the ternary heap.
    /// </summary>
    private static int GetParentIndex(int childIndex) => (childIndex - 1) / 3;

    /// <summary>
    /// Swaps two elements in the underlying storage.
    /// </summary>
    private void Swap(int i, int j)
    {
        T temp = _items[i];
        _items[i] = _items[j];
        _items[j] = temp;
    }

    /// <summary>
    /// Compares two elements using their IComparable implementation.
    /// </summary>
    private static int Compare(T a, T b) => a.CompareTo(b);
}