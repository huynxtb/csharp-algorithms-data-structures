using System;
using System.Collections.Generic;

/// <summary>
/// Represents a Max-Priority Queue implemented using an unsorted List.
/// </summary>
/// <typeparam name="T">The type of elements in the priority queue. Must implement IComparable<T>.</typeparam>
public class MaxPriorityQueue<T>
    where T : IComparable<T>
{
    private List<T> _elements;

    /// <summary>
    /// Initializes a new instance of the <see cref="MaxPriorityQueue{T}"/> class.
    /// </summary>
    public MaxPriorityQueue()
    {
        _elements = new List<T>();
    }

    /// <summary>
    /// Gets the number of elements contained in the priority queue.
    /// </summary>
    public int Count
    {
        get { return _elements.Count; }
    }

    /// <summary>
    /// Gets a value indicating whether the priority queue is empty.
    /// </summary>
    public bool IsEmpty
    {
        get { return _elements.Count == 0; }
    }

    /// <summary>
    /// Adds an item to the priority queue.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void Enqueue(T item)
    {
        _elements.Add(item);
    }

    /// <summary>
    /// Removes and returns the item with the highest priority from the priority queue.
    /// </summary>
    /// <returns>The item with the highest priority.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the priority queue is empty.</exception>
    public T DequeueMax()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Cannot dequeue from an empty priority queue.");
        }

        int maxIndex = 0;
        for (int i = 1; i < _elements.Count; i++)
        {
            if (_elements[i].CompareTo(_elements[maxIndex]) > 0)
            {
                maxIndex = i;
            }
        }

        T maxItem = _elements[maxIndex];
        _elements.RemoveAt(maxIndex);
        return maxItem;
    }

    /// <summary>
    /// Returns the item with the highest priority from the priority queue without removing it.
    /// </summary>
    /// <returns>The item with the highest priority.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the priority queue is empty.</exception>
    public T PeekMax()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Cannot peek at an empty priority queue.");
        }

        int maxIndex = 0;
        for (int i = 1; i < _elements.Count; i++)
        {
            if (_elements[i].CompareTo(_elements[maxIndex]) > 0)
            {
                maxIndex = i;
            }
        }

        return _elements[maxIndex];
    }
}