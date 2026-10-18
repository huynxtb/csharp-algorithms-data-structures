using System;
using System.Collections.Generic;

public class MinHeap<T>
{
    private List<T> _heap;
    private IComparer<T> _comparer;

    /// <summary>
    /// Initializes a new instance of the MinHeap class using the default comparer for type T.
    /// T must implement IComparable&lt;T&gt; or IComparable.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if type T does not implement IComparable&lt;T&gt; or IComparable.</exception>
    public MinHeap() : this(Comparer<T>.Default)
    {
        // Ensure T is comparable if default comparer is used.
        if (typeof(T).GetInterface(nameof(IComparable<T>)) == null && !typeof(IComparable).IsAssignableFrom(typeof(T)))
        {
            throw new InvalidOperationException("Type T must implement IComparable<T> or IComparable to use the default comparer.");
        }
    }

    /// <summary>
    /// Initializes a new instance of the MinHeap class using a custom comparer.
    /// </summary>
    /// <param name="comparer">The comparer to use for comparing elements in the heap.</param>
    /// <exception cref="ArgumentNullException">Thrown if the provided comparer is null.</exception>
    public MinHeap(IComparer<T> comparer)
    {
        _heap = new List<T>();
        _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
    }

    /// <summary>
    /// Gets the number of elements contained in the heap.
    /// </summary>
    public int Count => _heap.Count;

    /// <summary>
    /// Gets a value indicating whether the heap is empty.
    /// </summary>
    public bool IsEmpty => _heap.Count == 0;

    /// <summary>
    /// Adds an item to the heap while maintaining the min-heap property.
    /// </summary>
    /// <param name="item">The item to add to the heap.</param>
    public void Insert(T item)
    {
        _heap.Add(item);
        HeapifyUp();
    }

    /// <summary>
    /// Removes and returns the smallest item from the heap.
    /// </summary>
    /// <returns>The smallest item in the heap.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the heap is empty.</exception>
    public T ExtractMin()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Heap is empty.");
        }

        T minItem = _heap[0];
        int lastIndex = _heap.Count - 1;
        _heap[0] = _heap[lastIndex];
        _heap.RemoveAt(lastIndex);

        if (_heap.Count > 0)
        {
            HeapifyDown();
        }

        return minItem;
    }

    /// <summary>
    /// Returns the smallest item from the heap without removing it.
    /// </summary>
    /// <returns>The smallest item in the heap.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the heap is empty.</exception>
    public T PeekMin()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Heap is empty.");
        }
        return _heap[0];
    }

    private int GetParentIndex(int childIndex) => (childIndex - 1) / 2;
    private int GetLeftChildIndex(int parentIndex) => 2 * parentIndex + 1;
    private int GetRightChildIndex(int parentIndex) => 2 * parentIndex + 2;

    private bool HasParent(int index) => GetParentIndex(index) >= 0;
    private bool HasLeftChild(int index) => GetLeftChildIndex(index) < _heap.Count;
    private bool HasRightChild(int index) => GetRightChildIndex(index) < _heap.Count;

    private T GetParent(int index) => _heap[GetParentIndex(index)];
    private T GetLeftChild(int index) => _heap[GetLeftChildIndex(index)];
    private T GetRightChild(int index) => _heap[GetRightChildIndex(index)];

    private void Swap(int indexOne, int indexTwo)
    {
        T temp = _heap[indexOne];
        _heap[indexOne] = _heap[indexTwo];
        _heap[indexTwo] = temp;
    }

    private void HeapifyUp()
    {
        int currentIndex = _heap.Count - 1;
        while (HasParent(currentIndex) && _comparer.Compare(GetParent(currentIndex), _heap[currentIndex]) > 0)
        {
            Swap(currentIndex, GetParentIndex(currentIndex));
            currentIndex = GetParentIndex(currentIndex);
        }
    }

    private void HeapifyDown()
    {
        int currentIndex = 0;
        while (HasLeftChild(currentIndex))
        {
            int smallerChildIndex = GetLeftChildIndex(currentIndex);
            if (HasRightChild(currentIndex) && _comparer.Compare(GetRightChild(currentIndex), GetLeftChild(currentIndex)) < 0)
            {
                smallerChildIndex = GetRightChildIndex(currentIndex);
            }

            if (_comparer.Compare(_heap[currentIndex], _heap[smallerChildIndex]) < 0)
            {
                break; // Current node is smaller than its smallest child, heap property is maintained.
            }
            else
            {
                Swap(currentIndex, smallerChildIndex);
            }
            currentIndex = smallerChildIndex;
        }
    }
}