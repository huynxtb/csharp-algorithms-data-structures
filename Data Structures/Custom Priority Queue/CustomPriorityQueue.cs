using System;
using System.Collections.Generic;

public class CustomPriorityQueue<T>
{
    private class HeapNode
    {
        public T Value { get; set; }
        public int Priority { get; set; }

        public HeapNode(T value, int priority)
        {
            Value = value;
            Priority = priority;
        }
    }

    private List<HeapNode> heap;
    private bool isMaxHeap;

    public CustomPriorityQueue(bool isMaxHeap = true)
    {
        heap = new List<HeapNode>();
        this.isMaxHeap = isMaxHeap;
    }

    public int Count
    {
        get { return heap.Count; }
    }

    public bool IsEmpty
    {
        get { return heap.Count == 0; }
    }

    public void Enqueue(T item, int priority)
    {
        HeapNode newNode = new HeapNode(item, priority);
        heap.Add(newNode);
        HeapifyUp(heap.Count - 1);
    }

    public T Dequeue()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Priority queue is empty.");
        }

        T result = heap[0].Value;
        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);

        if (heap.Count > 0)
        {
            HeapifyDown(0);
        }

        return result;
    }

    public T Peek()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Priority queue is empty.");
        }

        return heap[0].Value;
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2;

            if (ShouldSwap(heap[index].Priority, heap[parentIndex].Priority))
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

    private void HeapifyDown(int index)
    {
        int lastIndex = heap.Count - 1;

        while (index < lastIndex)
        {
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;
            int targetIndex = index;

            if (leftChild <= lastIndex && ShouldSwap(heap[leftChild].Priority, heap[targetIndex].Priority))
            {
                targetIndex = leftChild;
            }

            if (rightChild <= lastIndex && ShouldSwap(heap[rightChild].Priority, heap[targetIndex].Priority))
            {
                targetIndex = rightChild;
            }

            if (targetIndex != index)
            {
                Swap(index, targetIndex);
                index = targetIndex;
            }
            else
            {
                break;
            }
        }
    }

    private bool ShouldSwap(int childPriority, int parentPriority)
    {
        if (isMaxHeap)
        {
            return childPriority > parentPriority;
        }
        else
        {
            return childPriority < parentPriority;
        }
    }

    private void Swap(int index1, int index2)
    {
        HeapNode temp = heap[index1];
        heap[index1] = heap[index2];
        heap[index2] = temp;
    }
}