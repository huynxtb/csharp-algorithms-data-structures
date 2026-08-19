using System;
using System.Collections.Generic;

public class PriorityQueue<TElement, TPriority>
{
    private readonly List<(TElement Element, TPriority Priority)> _heap;
    private readonly IComparer<TPriority> _comparer;

    public PriorityQueue() : this(Comparer<TPriority>.Default)
    {
    }

    public PriorityQueue(IComparer<TPriority> comparer)
    {
        _heap = new List<(TElement Element, TPriority Priority)>();
        _comparer = comparer ?? Comparer<TPriority>.Default;
    }

    public int Count => _heap.Count;

    public bool IsEmpty() => _heap.Count == 0;

    public void Enqueue(TElement element, TPriority priority)
    {
        _heap.Add((element, priority));
        HeapifyUp(_heap.Count - 1);
    }

    public TElement Dequeue()
    {
        if (IsEmpty())
        {
            throw new InvalidOperationException("The priority queue is empty.");
        }

        TElement element = _heap[0].Element;
        int lastIndex = _heap.Count - 1;
        _heap[0] = _heap[lastIndex];
        _heap.RemoveAt(lastIndex);

        if (_heap.Count > 0)
        {
            HeapifyDown(0);
        }

        return element;
    }

    public TElement Peek()
    {
        if (IsEmpty())
        {
            throw new InvalidOperationException("The priority queue is empty.");
        }
        return _heap[0].Element;
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;
            if (_comparer.Compare(_heap[index].Priority, _heap[parentIndex].Priority) >= 0)
            { 
                break; 
            }
            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    private void HeapifyDown(int index)
    {
        int lastIndex = _heap.Count - 1;
        while (true)
        {
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;
            int smallest = index;

            if (leftChild <= lastIndex && _comparer.Compare(_heap[leftChild].Priority, _heap[smallest].Priority) < 0)
            {
                smallest = leftChild;
            }

            if (rightChild <= lastIndex && _comparer.Compare(_heap[rightChild].Priority, _heap[smallest].Priority) < 0)
            {
                smallest = rightChild;
            }

            if (smallest == index)
            {
                break;
            }

            Swap(index, smallest);
            index = smallest;
        }
    }

    private void Swap(int index1, int index2)
    {
        var temp = _heap[index1];
        _heap[index1] = _heap[index2];
        _heap[index2] = temp;
    }
}